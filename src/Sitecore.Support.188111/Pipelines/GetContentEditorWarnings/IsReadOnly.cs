using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Support.Pipelines.GetContentEditorWarnings
{

    using Sitecore;
    using Sitecore.Data.Items;
    using Sitecore.Globalization;
    using System;
    using Sitecore.Pipelines.GetContentEditorWarnings;

    public class IsReadOnly
    {
        public void Process(GetContentEditorWarningsArgs args)
        {
            Item item = args.Item;
            if ((item != null) && item.Appearance.ReadOnly)
            {
                GetContentEditorWarningsArgs.ContentEditorWarning warning = args.Add();
                if (item.IsFallback)
                {
                    warning.Title = Translate.Text("No version exists in the current language. You see a fallback version from '{0}' language.", new object[] { item.OriginalLanguage });
                    warning.AddOption(Translate.Text("Navigate to the original item."), $"item:load(id={item.ID},language={item.OriginalLanguage},version=0,force=1)");
                    warning.AddOption(Translate.Text("Add a new version."), "item:addversion");
                }
                else
                {
                    warning.Title = Translate.Text("You cannot edit this item because it is protected.");
                    if (Context.IsAdministrator)
                    {
                        warning.Text = Translate.Text("To unprotect the item, click Unprotect Item on the Configure tab or click Unprotect Item.");
                        warning.AddOption(Translate.Text("Unprotect Item"), "item:togglereadonly");
                    }
                }

                warning.IsExclusive = false;
            }
        }
    }


}