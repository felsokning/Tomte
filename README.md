# Tomte
## Overview
Tomte is a combination of Windows PowerShell, WCF Client, Windows Service, WCF Service, and Windows Workflow Foundation (a.k.a.: Code Activities) to automate actions on remote windows machines.

## Name/Meaning
Tomte is named after the [nisse/tomte of Scaninavian folklore](https://en.wikipedia.org/wiki/Nisse_(folklore)#Ancestor_spirit) and the general idea is much the same: If Tomte is treated well,  it will aid in the chores and work.

## How It Works
Tomte exposes Windows Workflows via a WCF Service, which is wrapped by a Windows Service. The WCF Service is called by PowerShell commands, which wrap the WCF client. SQL Persistence is set-up in Tomte, so that all you have to do is stand-up a SQL server, modify your code to point to the SQL server, and it will take care of the rest. Adding new Activities to Tomte requires very little overhead, requiring just the PowerShell command to be written and the Activity to be added. Deploy the newly modified Windows Service and import the newly modified snap-in and that's it!

## PowerShell Commands and Activities
The bulk of Tomte's work is derived from [Activities](https://github.com/felsokning/Tomte/tree/master/Fels%C3%B6kning.Tomte/Fels%C3%B6kning.Tomte.WcfService/Activities) and [PowerShell Commands](https://github.com/felsokning/Tomte/tree/master/Fels%C3%B6kning.Tomte/Fels%C3%B6kning.Tomte.PowerShell/Commands). The PowerShell Command wraps the WCF Client call to the WCF Service hosted in the Windows Service. The Activity is what is invoked, when the activity called upon is found in the relevant [dictionary](https://github.com/felsokning/Tomte/blob/master/Fels%C3%B6kning.Tomte/Fels%C3%B6kning.Tomte.WcfService/WorkflowService.svc.cs#L306)

## What About Security?
Security is abstracted through the Windows Service/WCF layer, as all requests passing through AuthN/AuthZ. This makes it ideal to set-up in a either a work-group or domain setting, as no authentication schemes need to be manually configured or changed for Tomte to work.

## Screenshots

![DemonstrationofPowerShellCommands](/ScreenShots/PowerShellExecution.PNG?raw=true "PowerShell Commands return the data that you request via the command from the remote endpoint.")

![SqlQueryResults](/ScreenShots/SqlQuery.PNG?raw=true "Executions can be queried by users or you can build a central monitoring service for long-running activities.")
