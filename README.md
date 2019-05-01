# Tomte
## Overview
Tomte is a combination of Windows PowerShell, WCF Client, Windows Service, WCF Service, and Windows Workflow Foundation (a.k.a.: Code Activities) to automate actions on remote windows machines.

## Name/Meaning
Tomte is named after the [nisse/tomte of Scaninavian folklore](https://en.wikipedia.org/wiki/Nisse_(folklore)#Ancestor_spirit) and the general idea is much the same: If Tomte is treated well,  it will aid in the chores and work.

## How It Works
Tomte exposes Windows Workflows via a WCF Service, which is wrapped by a Windows Service. The WCF Service is called by PowerShell commands, which wrap the WCF client. SQL Persistence is set-up in Tomte, so that all you have to do is stand-up a SQL server, modify your code to point to the SQL server, and it will take care of the rest. Adding new Activities to Tomte requires very little overhead, requiring just the PowerShell command to be written and the Activity to be added. Deploy the newly modified Windows Service and import the newly modified snap-in and that's it!

## Screenshots

![DemonstrationofPowerShellCommands](/ScreenShots/PowerShellExecution.png?raw=true "PowerShell Commands return the data that you request via the command from the remote endpoint.")

![SqlQueryResults](/ScreenShots/SqlQuery.png?raw=true "Executions can be queried by users or you can build a central monitoring service for long-running activities.")
