#!/usr/bin/env python3
"""Check the candidate preview bundle before any public NuGet upload."""
import sys
import zipfile
from pathlib import Path
from xml.etree import ElementTree as ET

PACKAGES = {
    "VSlices",
    "VSlices.Spaces",
    "VSlices.Work",
    "VSlices.Grounding.EntityFrameworkCore.Functional",
    "VSlices.Grounding",
    "VSlices.Grounding.EntityFrameworkCore",
    "VSlices.Views.Abstract",
}
NS = {"n": "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"}
# Direct package references that must remain visible to NuGet consumers.
# Keep this graph aligned with the seven packable .csproj files.
INTERNAL_DEPENDENCIES = {
    "VSlices": set(),
    "VSlices.Spaces": {"VSlices"},
    "VSlices.Work": {"VSlices", "VSlices.Spaces"},
    "VSlices.Grounding": {"VSlices.Work"},
    "VSlices.Grounding.EntityFrameworkCore.Functional": {"VSlices"},
    "VSlices.Grounding.EntityFrameworkCore": {
        "VSlices.Work", "VSlices.Grounding.EntityFrameworkCore.Functional",
    },
    "VSlices.Views.Abstract": {"VSlices.Spaces"},
}


def fail(message):
    raise SystemExit(message)

if len(sys.argv) != 3:
    fail("Usage: verify-nuget-packages.py <artifacts-folder> <expected-version>")
folder = Path(sys.argv[1])
VERSION = sys.argv[2]
if not VERSION:
    fail("Expected package version cannot be empty")
files = sorted(folder.glob("*.nupkg"))
if len(files) != len(PACKAGES):
    fail(f"Expected {len(PACKAGES)} packages; found {len(files)}: {[x.name for x in files]}")
seen = set()
for path in files:
    with zipfile.ZipFile(path) as bundle:
        names = bundle.namelist()
        manifest = next((x for x in names if x.endswith(".nuspec")), None)
        if manifest is None:
            fail(f"Missing nuspec in {path.name}")
        root = ET.fromstring(bundle.read(manifest))
        ns = {"n": root.tag.split("}")[0][1:]} if root.tag.startswith("{") else {}
        def val(key):
            entry = root.find(f".//n:metadata/n:{key}", ns) if ns else root.find(f".//metadata/{key}")
            return entry.text if entry is not None else None
        package_id = val("id")
        version = val("version")
        authors = val("authors")
        if package_id not in PACKAGES or package_id in seen:
            fail(f"Unexpected or duplicate package ID: {package_id}")
        if version != VERSION:
            fail(f"{package_id}: unexpected version {version}")
        expected_authors = (
            ["Hernán Alvarez", "VSlices Development"]
            if package_id == "VSlices.Grounding.EntityFrameworkCore.Functional"
            else ["Enryu20", "Limen"]
        )
        actual_authors = [a.strip() for a in (authors or "").replace(";", ",").split(",")]
        if actual_authors != expected_authors:
            fail(f"{package_id}: unexpected authors metadata {authors!r}")
        dependencies = {
            element.attrib["id"]: element.attrib.get("version", "")
            for element in root.iter()
            if element.tag.split("}")[-1] == "dependency" and "id" in element.attrib
        }
        actual_internal = set(dependencies) & PACKAGES
        expected_internal = INTERNAL_DEPENDENCIES[package_id]
        if actual_internal != expected_internal:
            fail(f"{package_id}: internal dependencies {actual_internal} != {expected_internal}")
        for dependency_id in expected_internal:
            declared = dependencies[dependency_id]
            # NuGet renders ProjectReference versions as ranges, usually [x, ).
            minimum = declared.lstrip("[(").split(",", 1)[0].rstrip("])")
            if minimum != VERSION:
                fail(f"{package_id}: {dependency_id} version {declared!r} != {VERSION}")
        if "VSlices.EntityFrameworkCore.Functional" in dependencies:
            fail(f"{package_id}: reference to retired functional EF Core package ID")
        if "README.md" not in names or "icon.png" not in names:
            fail(f"{package_id}: missing package README or icon")
        if any(x.startswith("analyzers/") for x in names):
            fail(f"{package_id}: experimental analyzer was packaged")
        if not any(x.startswith("lib/net10.0/") and x.endswith(".dll") for x in names):
            fail(f"{package_id}: missing .NET 10 library assembly")
        seen.add(package_id)
        print(f"Verified {package_id} {version}")
if seen != PACKAGES:
    fail(f"Missing package IDs: {PACKAGES - seen}")
print("All seven packages verified. This does NOT establish NuGet.org ID ownership or publish them.")
