Microsoft keeps going on their Open Source Cross Platform woot train.

[Scott Hanselman has more details.](http://www.hanselman.com/blog/ASPNET5AndNETCoreRC1InContextPlusAllTheConnect2015News.aspx)  Read on for a summary from yours truly.

**Visual Studio Code** (like Sublime Text, Github Atom, etc., not an IDE) is Open Source, and in a new Beta that supports Extensions.  Go support, Yeoman, and Open File in Vim (for your command line friends) are some featured extensions.  It is cross platform too!

**ASP.NET 5 RC1** is the cross platform release candidate for that Microsoft web framework thing that no one uses outside of Windows.   The "5" part actually means you can develop, host, and run ASP.NET from Linux and OS X.  It has a "Go Live" license which means if you want to deploy it to Production in Linux or OS X, Microsoft will support you.  Documentation is pretty wizard too.  Docs are still under works, but does a wizard ever really stop working?  (Is wizard still a thing?  Yes?  Good.)

If you absolutely have to do it right now, *it's dangerous to go alone! Take this* : http://get.asp.net/.  The site should detect your OS and tell you how to get ASP.NET.  On my phone, it just said view source.  Maaan, I wanted ASP.NET running off of my Android 6.0 phone.

*How are you natively developing and running .NET bits outside of Windows?! Sorcery?!*

Naw, this thing called **.NET Core** is, you guessed it, the core bits of the .NET framework, excluding things that tie directly to the Windows OS \**cough\** WPF \**cough\** WinForms.  Not sure anyone outside Windows wants those anyway.  Development of the Core is primarily driven by ASP.NET 5 workloads, but it aims to fulfill the desire for a modular runtime where features and libraries can be cherry picked.  It is the "If you ask for a banana, you get only a banana and NOT the Gorilla and NOT the rest of the jungle" kind of thing.  http://docs.asp.net/en/latest/conceptual-overview/dotnetcore.html

The **.NET Execution Environment (DNX)** is the money.  It feels like being a magician.  "Hey Rocky, watch me run this Microsoft application in my RedHat".
![](/content/images/2015/11/bullwinkle-redhat.png)
(Okay, I'll stop)

The DNX is where you pick the .NET bits you want available for your runtime environment on your target platform.  It is the SDK for your application.  The DNX Version Manager (DNVM) allows you to wrangle your many DNX versions and flavors, and you can switch between them a la command line.
http://docs.asp.net/en/latest/dnx/overview.html

Why would you want to run Microsoft code on Linux?  Maybe that is what your IT staff knows.  Maybe you do not want to have that one Microsoft box in the corner.  The DNX and .NET Core lets you treat Linux as just another place to run your bits.  Well, CentOS and CentOS derivations are not supported yet as of RC1's known issues.

I rip off Scott Hanselman's closing remarks from [his blog post about all of this](http://www.hanselman.com/blog/ASPNET5AndNETCoreRC1InContextPlusAllTheConnect2015News.aspx).

>WHAT DOES IT ALL MEAN?

> It means that you can build basically whatever you want, however you want. You can use the editor you like, the OS you like, and the languages you like. VSCode on a Mac doing Node and deploying to Azure? Check. ASP.NET 5 with C# to [Docker Containers in a bunch of VMs created in Azure and managed with Microsoft Operations Manager](http://blogs.technet.com/b/momteam/archive/2015/11/04/oms-agent-for-linux-now-available.aspx)? Check. And on and on. Node.js on VS, C to Raspberry Pi's in C in VS, whatever you dig. It's a whole new world. 
