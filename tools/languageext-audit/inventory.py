#!/usr/bin/env python3
"""LanguageExt migration inventory. Static lexical census, not a Roslyn symbol binding."""
from __future__ import annotations
import argparse, csv, json, re
from pathlib import Path
from collections import Counter, defaultdict

ROOT = Path(__file__).resolve().parents[2]
OUT = ROOT / "docs/audits/languageext-2026-10"
EXTS = {".cs",".csproj",".props",".targets",".razor",".yml",".yaml"}
SYMBOLS = ("FinT Fin EitherT Either OptionT Option IO Eff K K2 Unit Error Seq Lst Arr "
           "HashMap HashSet TryOption TryT Try Pure ReaderT Reader StateT State WriterT Writer "
           "StreamT ValidationT Validation Atom MonadUnliftIO MonadIO MonadT Monad Functor "
           "Applicative Alternative Fallible Final SemigroupK Semigroup MonoidK Monoid "
           "Foldable Traversable Readable Writable Stateful Hashable Ord Eq ArrowApply Kleisli "
           "Aff Prelude OptionUnsafe Optional EitherUnsafe").split()
PRELUDE = ("Some None Left Right unit pure fail lift liftIO Optional Success Failure "
           "FinT Fin IO SomeUnsafe RightUnsafe LeftUnsafe toFin toOption").split()
EXTENSIONS = ("Bind Map Match MatchAsync MapFail MapError FailSpan ThrowIfFail IfNone "
              "RunFinT RunFin RunIO RunAsync Run As Fold Traverse Sequence SelectMany "
              "Select Flatten Iter Filter ForAll Do Choose ToFin ToOption ToEither "
              "IsSome IsNone IsFail IsSucc Lift MapIO MapT BindT BiMap").split()
TOKENS = re.compile(r"\\b(?:"+"|".join(map(re.escape,SYMBOLS))+r")\\b")
PRELUDE_CALL = re.compile(r"\\b(?:"+"|".join(map(re.escape,PRELUDE))+r")\\b(?=\\s*(?:\\(|<))")
EXTENSION_CALL = re.compile(r"\\.\\s*(?:"+"|".join(map(re.escape,EXTENSIONS))+r")\\b(?=\\s*(?:\\(|<))")
NAMESPACE = re.compile(r"\\bLanguageExt(?:\\.[A-Za-z_]\\w*)*")
PACKAGE = re.compile(r'<PackageReference\\s+Include="([^"]+)"(?:[^>]*?Version="([^"]+)")?')
GLOBAL_USING = re.compile(r'<Using\\s+Include="([^"]+)"([^>]*)>')
PROJECT_REF = re.compile(r'<ProjectReference\\s+Include="([^"]+)"')
DECL = re.compile(r'\\b(?:class|interface|struct|enum|record(?:\\s+(?:struct|class))?)\\s+([A-Za-z_]\\w*)')
CATEGORIES = {"explicit_namespace":"Certain textual reference","package":"Declared package",
              "implicit_import":"MSBuild global import","candidate_type":"Type or trait candidate",
              "candidate_prelude":"Prelude invocation candidate","candidate_extension":"Extension candidate"}
def project_of(path):
    pieces=Path(path).parts
    return "/".join(pieces[:3]) if len(pieces)>2 and pieces[0] in ("src","tests","examples") else pieces[0]
def main():
    parser=argparse.ArgumentParser()
    parser.add_argument("--root",type=Path,default=ROOT)
    parser.add_argument("--out",type=Path,default=OUT)
    args=parser.parse_args()
    root=args.root.resolve()
    out=args.out.resolve()
    out.mkdir(parents=True,exist_ok=True)
    records=[]; packages=[]; imports=[]; refs=[]; own=set(); covered=[]
    files=sorted(p for p in root.rglob("*") if p.is_file() and p.suffix in EXTS
                 and not any(x in {"bin","obj",".git","node_modules","artifacts"} for x in p.parts)
                 and out not in p.parents)
    for p in files:
        rel=p.relative_to(root).as_posix()
        text=p.read_text(encoding="utf-8-sig",errors="replace")
        covered.append(rel)
        for m in DECL.finditer(text):
            own.add(m.group(1))
        for line_no,line in enumerate(text.splitlines(),1):
            hits=[]
            for m in NAMESPACE.finditer(line):
                hits.append(("explicit_namespace",m.group(),m.start()+1))
            for m in PACKAGE.finditer(line):
                packages.append({"project":rel,"line":line_no,"package":m.group(1),"version":m.group(2)})
                if "LanguageExt" in m.group(1): hits.append(("package",m.group(1),m.start()+1))
            for m in GLOBAL_USING.finditer(line):
                imports.append({"project":rel,"line":line_no,"namespace":m.group(1),"attributes":m.group(2).strip()})
                if m.group(1).startswith("LanguageExt"): hits.append(("implicit_import",m.group(1),m.start()+1))
            for m in PROJECT_REF.finditer(line):
                refs.append({"project":rel,"line":line_no,"reference":m.group(1)})
            if p.suffix in {".cs",".razor"}:
                for m in TOKENS.finditer(line):
                    if m.group()=="K" and not re.search(r"\\bK\\s*<",line[m.start():]): continue
                    hits.append(("candidate_type",m.group(),m.start()+1))
                for m in PRELUDE_CALL.finditer(line):
                    hits.append(("candidate_prelude",m.group(),m.start()+1))
                for m in EXTENSION_CALL.finditer(line):
                    hits.append(("candidate_extension",m.group().lstrip(".").strip(),m.start()+1))
            for kind,symbol,col in hits:
                records.append({"path":rel,"line":line_no,"column":col,"kind":kind,"symbol":symbol,
                                "confidence":"certain" if kind in {"explicit_namespace","package","implicit_import"} else "requires_symbol_resolution",
                                "source":line.strip()[:400]})
    def write(name,rows,columns):
        with (out/name).open("w",encoding="utf-8",newline="") as handle:
            w=csv.DictWriter(handle,fieldnames=columns);w.writeheader();w.writerows(rows)
    write("all-locations.csv",records,["path","line","column","kind","symbol","confidence","source"])
    write("packages.csv",packages,["project","line","package","version"])
    write("msbuild-imports.csv",imports,["project","line","namespace","attributes"])
    write("project-references.csv",refs,["project","line","reference"])
    write("files-scanned.csv",[{"path":p} for p in covered],["path"])
    symbols=Counter(r["symbol"] for r in records if r["kind"] in {"candidate_type","candidate_prelude","candidate_extension"})
    by_file=defaultdict(list)
    for r in records: by_file[r["path"]].append(r)
    counters=Counter(r["kind"] for r in records)
    explicit=sorted({r["path"] for r in records if r["kind"] in {"explicit_namespace","package","implicit_import"}})
    meta={"root_commit":"PINNED_BY_GITHUB_ACTION_CHECKOUT","source_files":len(files),
          "location_rows":len(records),"files_with_candidates":len(by_file),
          "explicit_files":len(explicit),"counts":dict(counters),
          "local_declarations_colliding_with_candidates":sorted(own & set(SYMBOLS))}
    (out/"summary.json").write_text(json.dumps(meta,indent=2,ensure_ascii=False)+"\\n",encoding="utf8")
    lines=["# LanguageExt usage census", "",
           "**Method:** exhaustive lexical scan of tracked-like source/config files; NOT Roslyn-bound symbol attribution.",
           "Files and line numbers refer to the pinned checkout commit.", "",
           f"- Files scanned: **{len(files)}**",
           f"- Candidate/reference location rows: **{len(records)}**",
           f"- Files with matches: **{len(by_file)}**",
           f"- Files with explicit LanguageExt imports/references: **{len(explicit)}**", "",
           "## Classification and caveats", "",
           "- **Certain**: textual namespace reference, project package declaration, MSBuild global import.",
           "- **Candidate**: a type/trait token, Prelude invocation or extension-style method. Some are VSlices-owned or BCL; resolve with Roslyn before assigning origin.",
           "- Implicit and transitive usages are not fully attributable without compilation and symbol binding.",
           "- String literals, comments and documentation embedded in C# are included in the lexical census and need filtering.", "",
           "## Candidate symbol frequencies", "",
           "| Symbol | Occurrences |","|---|---:|"]
    lines.extend(f"| `{k}` | {v} |" for k,v in symbols.most_common())
    lines.extend(["","## File index","","| File | Lines with matches | Raw matches |","|---|---:|---:|"])
    for path,rr in sorted(by_file.items()):
        lines.append(f"| [`{path}`](../../../../{path}) | {len(set(r['line'] for r in rr))} | {len(rr)} |")
    lines.extend(["","## Outputs","","- `all-locations.csv`: file, line, column, symbol, classification, source snippet.",
                  "- `packages.csv`: dependency declarations.","- `msbuild-imports.csv`: imported global namespace entries.",
                  "- `project-references.csv`: graph edges.","- `files-scanned.csv`: coverage evidence.",
                  "- `summary.json`: counters and collisions.","",
                  "Do not treat matching a token such as `Map`, `Error`, `Fin` or `Monad` as proof of LanguageExt ownership.",
                  "Separate reproduced implementation code from conceptual inspiration for licensing/attribution."])
    (out/"README.md").write_text("\\n".join(lines)+"\\n",encoding="utf8")
    print(json.dumps(meta,ensure_ascii=False))
if __name__=="__main__":main()
