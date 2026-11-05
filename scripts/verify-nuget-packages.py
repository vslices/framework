#!/usr/bin/env python3
"""Check the candidate preview bundle before any public NuGet upload."""
import sys
import zipfile
from pathlib import Path
from xml.etree import ElementTree as ET

VERSION = "0.1.0-preview.1"
PACKAGES = {
    "VSlices",
    "VSlices.Spaces",
    "VSlices.Spaces.Services",
    "VSlices.Spaces.Products",
    "VSlices.Work",
    "VSlices.Work.Services",
    "VSlices.Work.Products",
    "VSlices.Grounding",
    "VSlices.Grounding.EntityFrameworkCore",
    "VSlices.Views.Abstract",
}
NS = {"n": "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"}

def fail(message):
    raise SystemExit(message)

folder = Path(sys.argv[1] if len(sys.argv) > 1 else "artifacts/nuget")
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
        if package_id not in PACKAGES or package_id in seen:
            fail(f"Unexpected or duplicate package ID: {package_id}")
        if version != VERSION:
            fail(f"{package_id}: unexpected version {version}")
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
print("All ten packages verified. This does NOT establish NuGet.org ID ownership or publish them.")
