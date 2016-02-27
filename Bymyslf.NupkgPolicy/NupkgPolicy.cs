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

        // This string identifies the type of policy. It is displayed in the
        // policy list when you add a new policy to a Team Project.
        public override string Type
        {
            get { return "Nuget Packages Policy"; }
        }

        // This string is a description of the type of policy. It is displayed
        // when you select the policy in the Add Check-in Policy dialog box.
        public override string TypeDescription
        {
            get { return "This policy will prevent user to check-in Nuget Packages"; }
        }

        // This method is called by the policy framework when you create
        // a new check-in policy or edit an existing check-in policy.
        // You can use this to display a UI specific to this policy type
        // allowing the user to change the parameters of the policy.
        public override bool Edit(IPolicyEditArgs args)
        {
            // Do not need any custom configuration
            return true;
        }

        // This method performs the actual policy evaluation.
        // It is called by the policy framework at various points in time
        // when policy should be evaluated. In this example, the method
        // is invoked when various asyc events occur that may have
        // invalidated the current list of failures.
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

        // This method is called if the user double-clicks on
        // a policy failure in the UI. In this case a message telling the user
        // to supply some comments is displayed.
        public override void Activate(PolicyFailure failure)
        {
            MessageBox.Show("Please exclude Nuget Packages from your check-in.", "How to fix your policy failure");
        }

        // This method is called if the user presses F1 when a policy failure
        // is active in the UI. In this example, a message box is displayed.
        public override void DisplayHelp(PolicyFailure failure)
        {
            MessageBox.Show("This policy helps you to remember to exclude Nuget Packages from your check-ins.", "Prompt Policy Help");
        }
    }
}