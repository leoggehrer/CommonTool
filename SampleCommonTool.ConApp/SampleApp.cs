
namespace SampleCommonTool.ConApp
{
    using CommonTool;
    public partial class SampleApp : ConsoleApplication
    {
        #region override methods
        /// <summary>
        /// Creates an array of menu items for the application menu.
        /// </summary>
        /// <returns>An array of MenuItem objects representing the menu items.</returns>
        protected override MenuItem[] CreateMenuItems()
        {
            var mnuIdx = 0;
            var menuItems = new List<MenuItem>
            {
                CreateMenuSeparator(),
                new()
                {
                    Key = $"{++mnuIdx}",
                    Text = ToLabelText("Force", "Change force flag"),
                    Action = (self) => ChangeForce(),
                },
                new()
                {
                    Key = $"{++mnuIdx}",
                    Text = ToLabelText("Depth", "Change max sub path depth"),
                    Action = (self) => ChangeMaxSubPathDepth(),
                },
                new()
                {
                    Key = $"{++mnuIdx}",
                    Text = ToLabelText("Projects path", "Change projects path"),
                    Action = (self) =>
                    {
                        var savePath = SourcePath;

                        SourcePath = SelectOrChangeToSubPath(SourcePath, MaxSubPathDepth, [ SourcePath ]);
                    },
                },
                CreateMenuSeparator(),
            };

            if (mnuIdx % 10 != 0)
            {
                mnuIdx += 10 - (mnuIdx % 10);
            }

            var paths = TemplatePath.GetSubPaths(SourcePath, MaxSubPathDepth + 1)
                                    .Where(p => TemplatePath.ContainsFiles(p, "*.cs"))
                                    .OrderBy(p => p)
                                    .ToArray();

            menuItems.AddRange(CreatePageMenuItems(ref mnuIdx, paths, (item, menuItem) =>
            {
                var subPath = item.Replace(SourcePath, string.Empty);

                menuItem.Text = ToLabelText("Path", $"{subPath}");
                menuItem.Tag = "path";
                menuItem.Action = (self) => {};
                menuItem.Params = new() { { "sourcePath", item }, { "subPath", subPath } };
            }));
            return [.. menuItems.Union(CreateExitMenuItems())];
        }
        #endregion override methods
    }
}
