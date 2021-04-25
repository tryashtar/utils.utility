using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryashtarUtils.Utility
{
    public static class IOUtils
    {
        // GetFiles but with multiple allowed file extensions
        public static IEnumerable<FilePath> GetValidFiles(FilePath directory, IEnumerable<string> extensions, bool recursive)
        {
            return ScanFiles(directory, new FilePath(), extensions, recursive, new FilePath[0]);
        }

        public static IEnumerable<FilePath> GetValidFiles(FilePath directory, IEnumerable<string> extensions, bool recursive, IEnumerable<FilePath> exclude)
        {
            return ScanFiles(directory, new FilePath(), extensions, recursive, exclude);
        }

        private static IEnumerable<FilePath> ScanFiles(FilePath original_path, FilePath relative_deeper, IEnumerable<string> extensions, bool recursive, IEnumerable<FilePath> exclude)
        {
            string dir = String.Join(Path.DirectorySeparatorChar.ToString(), original_path.CombineWith(relative_deeper));
            var valid = Directory.GetFiles(dir, "*.*")
                .Where(x => extensions.Contains(Path.GetExtension(x).ToLower()))
                .OrderBy(x => x, LogicalStringComparer.Instance) // sort entries by logical comparer
                .Select(x => new FilePath(x));
            if (recursive)
            {
                foreach (var subfolder in Directory.GetDirectories(dir))
                {
                    string name = Path.GetFileName(subfolder);
                    var final = original_path.CombineWith(relative_deeper).CombineWith(name);
                    if (!exclude.Any(x => x.StartsWith(final)))
                    {
                        var more = ScanFiles(original_path, relative_deeper.CombineWith(name), extensions, true, exclude);
                        valid = valid.Concat(more);
                    }
                }
            }
            return valid;
        }
    }
}
