#!/usr/bin/env python3
"""Stage a namespace-only, provenance-preserving extraction from the pinned LanguageExt submodule.

A pilot can stage all source files temporarily; a nonempty --selection file limits
the physical extracted copy to an explicit upstream source-file manifest.
This tool does NOT select dependencies automatically and cannot claim minimality.
"""
from __future__ import annotations
import argparse
import hashlib
import json
from pathlib import Path
import re
import shutil
import subprocess

ROOT = Path(__file__).resolve().parents[2]
UPSTREAM = ROOT / "language-ext"
CORE = UPSTREAM / "LanguageExt.Core"
TARGET = ROOT / "src" / "VSlices" / "Fork"
ROOT_NAMES = ("src", "tests", "examples")


def namespace_rewrite(text: str) -> str:
    return re.sub(r"\bLanguageExt\b", "ForkExt", text)


def sha256(contents: bytes) -> str:
    return hashlib.sha256(contents).hexdigest()


def git_submodule_sha() -> str:
    return subprocess.check_output(
        ["git", "rev-parse", "HEAD"], cwd=UPSTREAM, text=True
    ).strip()


def source_files() -> list[Path]:
    return sorted(
        p for p in CORE.rglob("*.cs")
        if not any(x in {"bin", "obj"} for x in p.parts)
    )


def apply(selection: set[str] | None) -> None:
    if not CORE.is_dir() or not (UPSTREAM / "LICENSE.md").is_file():
        raise SystemExit("Pinned LanguageExt submodule has not been checked out.")
    source = source_files()
    paths = {
        p.relative_to(CORE).as_posix(): p
        for p in source
    }
    missing = (selection or set()) - set(paths)
    if missing:
        raise SystemExit(f"Unknown selected LanguageExt source paths: {sorted(missing)[:20]}")
    selected = paths if selection is None else {k: paths[k] for k in sorted(selection)}
    TARGET.mkdir(parents=True, exist_ok=True)
    for previous in TARGET.rglob("*.cs"):
        previous.unlink()
    entries = []
    for relative, original in selected.items():
        raw = original.read_bytes()
        text = raw.decode("utf-8-sig")
        rewritten = namespace_rewrite(text)
        destination = TARGET / relative
        destination.parent.mkdir(parents=True, exist_ok=True)
        destination.write_text(rewritten, encoding="utf-8", newline="")
        entries.append({
            "source": f"LanguageExt.Core/{relative}",
            "destination": f"src/VSlices/Fork/{relative}",
            "sha256_source": sha256(raw),
            "sha256_extracted": sha256(destination.read_bytes()),
        })
    license_file = TARGET / "LICENSE.LanguageExt.MIT"
    license_file.write_bytes((UPSTREAM / "LICENSE.md").read_bytes())
    readme = TARGET / "README.md"
    readme.write_text(
        "# ForkExt - source extracted from LanguageExt\n\n"
        "Original author: Paul Louth and LanguageExt contributors.\n"
        "Upstream: https://github.com/louthy/language-ext\n"
        f"Pinned VSlices submodule: https://github.com/HernanFAR/language-ext/tree/{git_submodule_sha()}\n\n"
        "The source files in this directory are derived from MIT-licensed LanguageExt.Core. "
        "Their namespace root is changed from LanguageExt to ForkExt. "
        "The original copyright notice and license are preserved in LICENSE.LanguageExt.MIT.\n\n"
        "MANIFEST.json records exact original file names and SHA-256 hashes. "
        "This folder is a third-party derived source surface, not a claim of VSlices authorship.\n",
        encoding="utf-8"
    )
    (TARGET / "MANIFEST.json").write_text(
        json.dumps({
            "upstream_commit": git_submodule_sha(),
            "license": "MIT",
            "original_copyright": "Copyright (c) 2014-2025 Paul Louth",
            "transformation": "LanguageExt identifier namespace root renamed to ForkExt",
            "selected_files": len(entries),
            "upstream_total_source_files": len(source),
            "entries": entries,
        }, indent=2) + "\n",
        encoding="utf-8"
    )
    consumer_files = 0
    for top in ROOT_NAMES:
        for item in (ROOT / top).rglob("*"):
            if item.is_file() and item.suffix in {".cs", ".razor", ".csproj"}:
                if TARGET in item.parents:
                    continue
                if any(x in {"bin", "obj"} for x in item.parts):
                    continue
                old = item.read_text(encoding="utf-8-sig")
                new = namespace_rewrite(old)
                if item == ROOT / "src/VSlices/VSlices.csproj":
                    new = re.sub(
                        r"^\s*<PackageReference\s+Include=\"LanguageExt.Core\"[^>]*/>\s*\n",
                        "\n", new, flags=re.MULTILINE)
                    # The replacement is also done after namespace_rewrite, which
                    # changes the literal package name in the csproj XML.
                    new = re.sub(
                        r"^\s*<PackageReference\s+Include=\"ForkExt.Core\"[^>]*/>\s*\n",
                        "\n", new, flags=re.MULTILINE)
                    new = new.replace(
                        "<Nullable>enable</Nullable>",
                        "<Nullable>enable</Nullable>\n"
                        "    <ImplicitUsings>disable</ImplicitUsings>\n"
                        "    <DefineConstants>$(DefineConstants);CONTRACTS_FULL</DefineConstants>"
                    )
                # The pinned upstream project builds without SDK implicit imports.
                # Avoid injecting System.Collections.Generic.HashSet<T> globally into ForkExt.
                # Keep normal BCL usings local to VSlices-authored source only.
                if item.suffix == ".cs" and (ROOT / "src/VSlices") in item.parents:
                    bcl = (
                        "using System;\\n"
                        "using System.Collections.Generic;\\n"
                        "using System.IO;\\n"
                        "using System.Linq;\\n"
                        "using System.Net.Http;\\n"
                        "using System.Threading;\\n"
                        "using System.Threading.Tasks;\\n"
                    )
                    new = bcl.replace("\\n", "\n") + new
                if new != old:
                    item.write_text(new, encoding="utf-8")
                    consumer_files += 1
    print(json.dumps({
        "copied": len(entries),
        "upstream_total": len(source),
        "consumer_files_updated": consumer_files,
        "upstream_revision": git_submodule_sha()
    }))


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--selection", type=Path, default=None)
    args = parser.parse_args()
    selection = None
    if args.selection is not None:
        selection = set()
        for line in args.selection.read_text(encoding="utf-8").splitlines():
            line = line.strip()
            if line and not line.startswith("#"):
                selection.add(line)
        if not selection:
            raise SystemExit("Selection must contain source paths; refusing empty extraction.")
    apply(selection)


if __name__ == "__main__":
    main()
