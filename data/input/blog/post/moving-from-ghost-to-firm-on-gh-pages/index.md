# Moving to Github Pages from a Self-Hosted Ghost on Azure

My blog setup as of this post is self-hosted instance of [Ghost](https://ghost.org) on a simple shared Azure site.  I have some Azure credits through my work which I am free to use, and the cost to host was minimal.  I published new posts infrequently so naturally my Ghost version fell behind.  Mine was version 0.5.5 while Ghost is v0.11.5 at this time.

To inspire myself to post more frequently, I decided to change platforms to something F# based.  I tried [FsBlog](https://github.com/fsprojects/FsBlog) first, but I got stuck.  Most likely it was a combination of trying to understand how the whole project worked and working while tired.  I finally settled on using [Firm](https://github.com/andagr/Firm) which I found after remembering to check the [F# Community Projects page](http://fsharp.org/community/projects/).

I have only four published posts on my Ghost blog, but I chose to take an indirect route to transfer them to my new Firm blog.

## A Summary of the Ghost Upgrade Steps

1. Try to use Ghost's backup feature to export all my Ghost posts.
2. Discover that the backup feature started in v0.5.6 but I am in v.0.5.5!
3. Try to use Azure to backup my entire deployed site before starting the Ghost upgrade process.
    * The Ghost upgrade ended up working without a problem, but I do not regret taking a backup.
4. Discover that Azure website backups are only available on the "Standard" tier while I am on the "Shared" tier.
5. Since I have ample free Azure credits, temporarily upgrade to the cheapest "Standard" tier which is "Standard: 1 Small".
6. From my "App Service" resource in Azure, go to Settings > Backups.
7. Configure the Storage and add a new "Storage account" to hold the backup.
8. I chose the default "Standard-LRS" because I wanted to backup the entire file system on the web app.
9. Since this is a one-time backup, no schedule.
10. Start the backup.  Notification said that it completed in two minutes, but the backup was not available to restore for almost fifteen minutes.
11. Restore the backup to a new web app instance in case something goes wrong to avoid trashing my Ghost blog.
12. Restore succeeds so delete the new instance.
13. Download Ghost 0.5.6 from their Github releases page and perform the upgrade to 0.5.6 per [their upgrade guide](https://support.ghost.org/how-to-upgrade/).
    * The guide mentions "npm-shrinkwrap.json" which I did not have, and I did not have to do the "Check your permissions" steps.  No issues.
14. Somehow performing a backup in Azure removed my Azure settings to automatically deploy updates from my master branch in Github to Azure.
15. Go to my blog App Service > App Deployment > Deployment options and add them again with the Setup button.
16. Ghost 0.5.6 now live in Azure.
17. Go to Admin > Settings.  Now the Labs page is there to let me export a json backup of my Ghost blog posts.
18. Export the json backup file!

## Porting the Ghost Posts to Firm

Now the interesting part - I need to transform the posts into the folder and file structre that Firm expects.  I could have done it by hand, but that would not have been fun.  I decided to have an F# script do the conversion for me using a [JSON Type Provider](http://fsharp.github.io/FSharp.Data/library/JsonProvider.html) pointed to my Ghost blog export json.  Writing the script certainly took longer than converting my four Ghost posts to Firm by hand would have taken, but it was more fun.

The abbreviated structure of my ghost backup json with some data type info is:

```json
{
    "db": [
        {
            "meta": {},
            "data": {
                "posts": [
                    {
                        "id": int,
                        "uuid": guid string,
                        "title": string,
                        "slug": string,
                        "markdown": string,
                        "html": string,
                        "image": ???,
                        "featured": int,
                        "page": int,
                        "status": string,
                        "language": string,
                        "meta_title": string,
                        "meta_description": string,
                        "author_id": int,
                        "created_at": int64 (unix-timestamp),
                        "created_by": int,
                        "updated_at": int64 (unix-timestamp),
                        "updated_by": int,
                        "published_at": int64 (unix-timestamp),
                        "published_by": int
                    }
                ],
            "don't care about the rest": []
            }
        }
    ]
}
```

[This is the FSX file.](https://github.com/gfritz/gfritz.github.io/blob/generate/blog-transfer.fsx)  For some reason the Posts type is NugetParams instead of a Posts TypeProvider type, however the code using the Posts works fine.  It might be a glitch in VS Code; I'm not sure.