#I @"packages\"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"
#r @"FAKE\tools\FakeLib.dll"

open System
open System.IO
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
    start.AddSeconds(float timestamp).ToLocalTime()

type OldBlog = JsonProvider< "garth-blog.ghost.2017-02-23.json">

let root = OldBlog.GetSample()

/// there is only one Db in a ghost blog backup json
let data = root.Db.[0].Data
let posts = data.Posts
let oldPostsData =
    posts
    |> Array.map (fun post ->
        let title = string post.Title
        let dirName = string post.Slug
        let dateUtc =
            Option.map toDateTimeFromUnixTimestamp post.PublishedAt
            |> function None -> DateTime.UtcNow | Some d -> d.ToUniversalTime()
        let markdown = string post.Markdown
        { title   = title;
          dirName = dirName;
          dateUtc = dateUtc;
          markdown = markdown; })
let createMetaJson oldPost =
    String.Format(
        """
        {
        "title": "{0}",
        "date":  "{1}",
        "tags": [ "blog"]
        }
        """, oldPost.title, oldPost.dateUtc)

let createBlogPost oldPost =
    let root = "data/input/blog/post/"
    let index = "index.md"
    let meta = "meta.json"
    FileUtils.mkdir oldPost.dirName
    FileUtils.pushd <| root + oldPost.dirName
    File.AppendAllLines(index, oldPost.markdown)
    File.AppendAllLines(meta, createMetaJson oldPost)
    FileUtils.popd