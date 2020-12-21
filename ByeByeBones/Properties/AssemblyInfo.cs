using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;

[assembly: AssemblyTitle(ByeByeBones.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(ByeByeBones.BuildInfo.Company)]
[assembly: AssemblyProduct(ByeByeBones.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + ByeByeBones.BuildInfo.Author)]
[assembly: AssemblyTrademark(ByeByeBones.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(ByeByeBones.BuildInfo.Version)]
[assembly: AssemblyFileVersion(ByeByeBones.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof(ByeByeBones.ByeByeBones), ByeByeBones.BuildInfo.Name, ByeByeBones.BuildInfo.Version, ByeByeBones.BuildInfo.Author, ByeByeBones.BuildInfo.DownloadLink)]


// Create and Setup a MelonModGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonModGameAttribute is found or any of the Values for any MelonModGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonModGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]