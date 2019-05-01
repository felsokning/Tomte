//-----------------------------------------------------------------------
// <copyright file="InstallSysInternalsActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    ///     Initializes a new instance of the <see cref="InstallSysInternalsActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class InstallSysInternalsActivity : SwedishCodeActivity<object>
    {
        /// <summary>
        ///     Private list of all of the tools to down.
        /// </summary>
        private readonly List<string> _items = new List<string>
        {
            "accesschk.exe","accesschk64.exe","AccessEnum.exe","AdExplorer.chm","ADExplorer.exe","ADInsight.chm",
            "ADInsight.exe","adrestore.exe","Autologon.exe","autoruns.chm","autoruns.exe","Autoruns64.dll","Autoruns64.exe",
            "autorunsc.exe","autorunsc64.exe","Bginfo.exe","Bginfo64.exe","Cacheset.exe","Clockres.exe","Clockres64.exe","Contig.exe",
            "Contig64.exe","Coreinfo.exe","ctrl2cap.amd.sys","ctrl2cap.exe","ctrl2cap.nt4.sys","ctrl2cap.nt5.sys","dbgview.chm",
            "Dbgview.exe","DEFRAG.EXE","Desktops.exe","Disk2vhd.chm","disk2vhd.exe","diskext.exe","diskext64.exe","Diskmon.exe",
            "DISKMON.HLP","DiskView.exe","DMON.SYS","du.exe","du64.exe","efsdump.exe","Eula.txt","FindLinks.exe","FindLinks64.exe",
            "handle.exe","handle64.exe","healthmonitoring.html","hex2dec.exe","hex2dec64.exe","junction.exe","junction64.exe",
            "ldmdump.exe","Listdlls.exe","Listdlls64.exe","livekd.exe","livekd64.exe","LoadOrd.exe","LoadOrd64.exe","LoadOrdC.exe",
            "LoadOrdC64.exe","logonsessions.exe","logonsessions64.exe","movefile.exe","movefile64.exe","notmyfault.exe","notmyfault64.exe",
            "notmyfaultc.exenotmyfaultc64.exe","ntfsinfo.exe","ntfsinfo64.exe","pagedfrg.exe","pagedfrg.hlp","pendmoves.exe",
            "pendmoves64.exe","pipelist.exe","pipelist64.exe","PORTMON.CNT","portmon.exe","PORTMON.HLP","procdump.exe","procdump64.exe",
            "procexp.chm","procexp.exe","procexp64.exe","procmon.chm","Procmon.exe","Procmon64.exe","psexec.exe","PsExec64.exe",
            "psfile.exe","psfile64.exe","psgetsid.exe","PsGetsid64.exe","Psinfo.exe","PsInfo64.exe","pskill.exe","pskill64.exe",
            "pslist.exe","pslist64.exe","psloggedon.exe","PsLoggedon64.exe","psloglist.exe","pspasswd.exe","pspasswd64.exe","psping.exe",
            "psping64.exe","psservice.exe","PsService64.exe","psshutdown.exe","pssuspend.exe","pssuspend64.exe","Pstools.chm",
            "psversion.txt","RAMMap.exe","readme.txt","RegDelNull.exe","RegDelNull64.exe","Reghide.exe","regjump.exe","RootkitRevealer.chm",
            "RootkitRevealer.exe","ru.exe","ru64.exe","sdelete.exe","sdelete64.exe","ShareEnum.exe","ShellRunas.exe","sigcheck.exe",
            "sigcheck64.exe","streams.exe","streams64.exe","strings.exe","strings64.exe","sync.exe","sync64.exe","Sysmon.exe","Sysmon64.exe",
            "tcpvcon.exe","tcpview.chm","Tcpview.exe","TCPVIEW.HLP","Testlimit.exe","Testlimit64.exe","Vmmap.chm","vmmap.exe","Volumeid.exe",
            "Volumeid64.exe","whois.exe","whois64.exe","Winobj.exe","WINOBJ.HLP","ZoomIt.exe"
        };

        /// <summary>
        ///     Private reference to the URL for the live tools.
        /// </summary>
        private string Url = "https://live.sysinternals.com/";

        /// <summary>
        ///     Overrides the <see cref="Execute"/> method exposed by the <see cref="SwedishCodeActivity{T}"/> class.
        /// </summary>
        /// <param name="context">The execution context passed when invoked.</param>
        /// <returns>A string back to the caller.</returns>
        /// <inheritdoc cref="SwedishCodeActivity{T}"/>
        protected override object Execute(CodeActivityContext context)
        {
            string workingPath = Environment.GetEnvironmentVariable("SystemDrive") + "\\SysInternals\\";
            if (!Directory.Exists(workingPath))
            {
                Directory.CreateDirectory(workingPath);
            }

            Parallel.ForEach(
                _items,
                item =>
                {
                    using (WebClient newWebClient = new WebClient())
                    {
                        newWebClient.DownloadFileAsync(new Uri(Url + item), workingPath + item);
                    }
                });

            // TODO: Should be a void task.
            return "Completed";
        }
    }
}