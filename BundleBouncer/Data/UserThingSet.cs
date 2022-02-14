/**
 * BundleBouncer User Allow/Blocklist Classes
 * 
 * Copyright (c) 2022 BundleBouncer Contributors
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */


using Org.BouncyCastle.Utilities.Encoders;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BundleBouncer.Data
{
    public abstract class UserThingSet<T>
    {
        protected HashSet<T> things = new HashSet<T>();

        public string[] FileHeader { get; internal set; }

        internal UserThingSet() { }
        internal UserThingSet(string filename, string singular, string plural, string listname, string[] fileHeader)
        {
            FileHeader = fileHeader;
            if (File.Exists(filename))
            {
                string line;
                foreach (var oline in File.ReadAllLines(filename))
                {
                    line = oline.Trim();
                    if (line == string.Empty)
                        continue;
                    if (line.StartsWith("#"))
                        continue;
                    AddThingFromString(line);
                }
            }
            else
            {
                SaveToFile(filename);
            }
            string items = things.Count == 1 ? singular : plural;
            Logging.Info($"Loaded {things.Count} {items} from {filename} into {listname}.");
        }

        internal void Add(T thing)
        {
            things.Add(thing);
        }

        internal void Remove(T thing)
        {
            things.Remove(thing);
        }

        internal bool Contains(T thing)
        {
            return things.Contains(thing);
        }

        internal void SaveToFile(string filename)
        {
            using (var w = new StreamWriter(filename))
            {
                if (FileHeader != null && FileHeader.Length == 0)
                {
                    foreach (var hdrl in FileHeader)
                    {
                        w.WriteLine($"# {hdrl}");
                    }
                    w.WriteLine();
                }
                foreach (var item in DumpToStrings())
                    w.WriteLine(item);
            }
        }

        internal abstract void AddThingFromString(string entry);
        internal abstract string[] DumpToStrings();
    }

    public class UserAvatarSet : UserThingSet<string>
    {
        public UserAvatarSet(string filename, string singular, string plural, string listname, string[] fileHeader) : base(filename, singular, plural, listname, fileHeader)
        {
        }

        // File.ReadAllLines(UserAvatarBlockListFile).Select(x => x.Trim().ToLowerInvariant()).Where(x => x != "" && !x.StartsWith("#")).ToHashSet();
        internal override void AddThingFromString(string entry)
        {
            Add(entry.ToLowerInvariant());
        }

        internal override string[] DumpToStrings()
        {
            return things.ToArray();
        }
    }

    public class UserHashSet : UserThingSet<byte[]>
    {
        public UserHashSet(string filename, string singular, string plural, string listname, string[] fileHeader) : base(filename, singular, plural, listname, fileHeader)
        {
        }

        internal override void AddThingFromString(string entry)
        {
            Add(Hex.Decode(entry));
        }

        internal override string[] DumpToStrings()
        {
            return things.Select(x => Hex.ToHexString(x).ToUpper()).ToArray();
        }
    }
}
