using RedstoneSidekick.Domain.MinecraftSchematics;
using RedstoneSidekick.Domain.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekick.Logic.StructureFiles
{
    public abstract class StructureProcessor
    {
        public abstract RedstoneSidekickProject CreateProjectFromFile(string filePath, string fileName);

        public abstract RedstoneSidekickProject AddStructureToProject(RedstoneSidekickProject project, string filePath, string fileName);

        protected static Dictionary<int, int> ParsePropertyBasedCounts(Dictionary<int, int> itemDictionary, IPalette palette, int itemId)
        {
            var itemCount = itemDictionary[itemId];

            if (palette.Properties != null)
            {
                string value;
                if (palette.Properties.TryGetValue("half", out value))          //Doors, 2 High Plants
                {
                    if (value != "upper")
                    {
                        itemCount++;
                    }
                }
                else if (palette.Properties.TryGetValue("part", out value))     //Beds
                {
                    if (value == "foot")
                    {
                        itemCount++;
                    }
                }
                else if (palette.Properties.TryGetValue("layers", out value))     //Snow Layers
                {
                    itemCount += int.Parse(value);
                }
                else if (palette.Properties.TryGetValue("type", out value))     //Slabs
                {
                    if (value == "bottom" || value == "top")
                    {
                        itemCount++;
                    }
                    else if (value == "double")
                    {
                        itemCount += 2;
                    }
                    else
                    {
                        itemCount++;
                    }
                }
                else if (palette.Properties.TryGetValue("pickles", out value))     //Sea Pickles
                {
                    itemCount += int.Parse(value);
                }
                else                                                            //Everything Else
                {
                    itemCount++;
                }
            }
            else
            {
                itemCount++;
            }

            itemDictionary[itemId] = itemCount;

            return itemDictionary;
        }
    }
}
