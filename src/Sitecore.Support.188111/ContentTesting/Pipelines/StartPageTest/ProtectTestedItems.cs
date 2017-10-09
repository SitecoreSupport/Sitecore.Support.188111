using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Support.ContentTesting.Pipelines.StartPageTest
{
    using Data;
    using Data.Items;
    using Diagnostics;
    using Sitecore.ContentTesting.Pipelines.StartPageTest;

    public class ProtectTestedItems : StartPageTestProcessor
    {
        public override void Process(StartPageTestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            if (args.Test != null)
            {
                foreach (Item item in args.Test.InnerItem.Axes.GetDescendants())
                {
                    if (item.Fields["Datasource"] != null)
                    {
                        Item testedItem = item.Database.GetItem(DataUri.Parse(item.Fields["Datasource"].Value));

                        if (testedItem != null)
                        {
                            testedItem.Editing.BeginEdit();
                            testedItem.Appearance.ReadOnly = true;
                            testedItem.Editing.EndEdit();
                        }
                        else
                        {
                            string errorMessage =
                            string.Format(
                                "Tested item is null. Please check if the \"Datasource\" field of the test definition item with ID = {0} is not empty.",
                                item.ID.ToString());

                            Log.Error(errorMessage, this);
                        }
                    }
                }
            }
        }
    }
}