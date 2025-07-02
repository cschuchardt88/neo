// Copyright (C) 2015-2025 The Neo Project.
//
// NeoWalletCommand.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System.Management.Automation;  // PowerShell namespace.

namespace Neo.PowerShell.Cmdlets
{
    // Declare the class as a cmdlet and specify and
    // appropriate verb and noun for the cmdlet name.
    [Cmdlet(VerbsCommon.Open, "Neo-Wallet")]
    public class NeoWalletCommand : Cmdlet
    {
        // Declare the parameters for the cmdlet.
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true
        )]
        public required string Path { get; set; }

        // Override the ProcessRecord method to process
        // the supplied user name and write out a
        // greeting to the user by calling the WriteObject
        // method.
        protected override void ProcessRecord()
        {
            WriteObject("Hello " + Path + "!");
        }
    }
}
