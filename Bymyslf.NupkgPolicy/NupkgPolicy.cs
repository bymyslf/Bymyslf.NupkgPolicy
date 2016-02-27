namespace Bymyslf.NupkgPolicy
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using Microsoft.TeamFoundation.VersionControl.Client;

    [Serializable]
    public class NupkgPolicy : PolicyBase
    {
        private const string NugetPackageFileExtension = ".nupkg";

        public override string Description
        {
            get { return "Prevent users to check-in Nuget Packages"; }
        }

        public override string Type
        {
            get { return "Nuget Packages Policy"; }
        }

        public override string TypeDescription
        {
            get { return "This policy will prevent user to check-in Nuget Packages"; }
        }

        public override bool Edit(IPolicyEditArgs args)
        {
            // Do not need any custom configuration
            return true;
        }

        public override PolicyFailure[] Evaluate()
        {
            var pendingChanges = PendingCheckin.PendingChanges.CheckedPendingChanges;
            foreach (var pendingChange in pendingChanges)
            {
                if (Path.GetExtension(pendingChange.LocalItem).ToLowerInvariant().Equals(NugetPackageFileExtension))
                {
                    return new PolicyFailure[] {
                        new PolicyFailure("Please exclude Nuget Packages from your check-in", this)
                    };
                }
            }

            return new PolicyFailure[0];
        }

        public override void Activate(PolicyFailure failure)
        {
            MessageBox.Show("Please exclude Nuget Packages from your check-in.", "How to fix your policy failure");
        }

        public override void DisplayHelp(PolicyFailure failure)
        {
            MessageBox.Show("This policy helps you to remember to exclude Nuget Packages from your check-ins.", "Prompt Policy Help");
        }
    }
}