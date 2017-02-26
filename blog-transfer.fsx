#I @"packages\"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"
#r @"FAKE\tools\FakeLib.dll"

open System
open System.IO
open System.Text
open FSharp.Data
open Fake

type BlogInfo = {
    title: string;
    dirName: string;
    dateUtc: DateTime;
    markdown: string;
}

let toDateTimeFromUnixTimestamp (timestamp:int64) =
    let start = DateTime(1970,1,1,0,0,0,DateTimeKind.Utc)
    start.AddMilliseconds(double timestamp).ToLocalTime()

type OldBlog = JsonProvider< "garth-blog.ghost.2017-02-23.json">

let root = OldBlog.GetSample()

/// there is only one Db in a ghost blog backup json
let data = root.Db.[0].Data
let posts = data.Posts
let oldPostsData =
    posts
    |> Array.map (fun post ->
        match post.PublishedAt with
        | None -> None
        | Some ts ->
            let title = string post.Title
            let dirName = string post.Slug
            let dateUtc =
                Option.map toDateTimeFromUnixTimestamp post.PublishedAt
                |> function None -> DateTime.UtcNow | Some d -> d.ToUniversalTime()
            let markdown =
                match post.Markdown with
                | Some md -> md
                | None -> "# Empty Content"
            Some { title   = title;
            dirName = dirName;
            dateUtc = dateUtc;
            markdown = markdown; })

/// http://www.fssnip.net/5O
let escapeString (str : string) =
   let buf = StringBuilder(str.Length)
   let replaceOrLeave c =
      match c with
      | '\r' -> buf.Append "\\r"
      | '\n' -> buf.Append "\\n"
      | '\t' -> buf.Append "\\t"
      | '"' -> buf.Append "\\\""
      | _ -> buf.Append c
   str.ToCharArray() |> Array.iter (replaceOrLeave >> ignore)
   buf.ToString()

let createMetaJson oldPost =
    String.Format(
        """
        {{
        "title": "{0}",
        "date":  "{1}",
        "tags": [ "blog"]
        }}
        """, escapeString oldPost.title, oldPost.dateUtc.ToString("r"))

let createBlogPost repoRoot oldPost =
    let postDir = repoRoot + "/data/input/blog/post/"
    let index = "index.md"
    let meta = "meta.json"
    printfn "dirName %s" oldPost.dirName
    FileUtils.cd postDir
    FileUtils.mkdir oldPost.dirName
    FileUtils.cd oldPost.dirName
    File.AppendAllLines(index, [oldPost.markdown])
    File.AppendAllLines(meta, [createMetaJson oldPost])
    FileUtils.cd postDir

let repoRoot = FileUtils.pwd ()

Array.choose id oldPostsData
|> (Array.iter (createBlogPost repoRoot))