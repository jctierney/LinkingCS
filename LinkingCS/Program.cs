/******************************************************************************
 * Authors: Jason Tierney | Erik Kremer
 * File:    Program.cs - meh, I should rename it...
 * Description: Linking program to create symbolic / hard links in Windows.
 *              Uses Win32 APIs, thus demonstrates both using these low-level
 *              APIs and how to use low level APIs within managed code, such
 *              as C#.
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace LinkingCS
{
    class Program
    {
        /// <summary>
        /// This is the Win32 API function used to create hard links.
        /// </summary>
        /// <param name="lpFileName"></param>
        /// <param name="lpExistingFileName"></param>
        /// <param name="lpSecurityAttributes"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        static extern bool CreateHardLink(
            string lpFileName,
            string lpExistingFileName,
            IntPtr lpSecurityAttributes
        );

        /// <summary>
        /// This is the Win32 API function used to create sym links.
        /// </summary>
        /// <param name="lpSymlinkFileName"></param>
        /// <param name="lpSymTargetFileName"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        static extern bool CreateSymbolicLink(
            string lpSymlinkFileName,
            string lpSymTargetFileName,
            uint dwFlags
        );

        const uint SYMBOLIC_LINK_FLAG_FILE = 0x0;
        const uint SYMBOLIC_LINK_FLAG_DIRECTORY = 0x1;

        /// <summary>
        /// Main entry point.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length > 2) // sym link
            {
                if (args[0].Equals("-s"))
                {
                    Console.WriteLine("Creating a symbolic link...");
                    if (File.Exists(args[1]))
                    {
                        FileInfo file = new FileInfo(args[2]);
                        FileInfo file2 = new FileInfo(args[1]);
                        Console.WriteLine(file.FullName);
                        if (CreateSymbolicLink(file.FullName, file2.FullName, SYMBOLIC_LINK_FLAG_FILE))
                        {
                            Console.WriteLine("Sym link create successfully...");
                        }
                        else
                        {
                            Console.WriteLine("Sym link wasn't create successfully...");
                        }

                        return;
                    }
                    else if (Directory.Exists(args[1]))
                    {
                        DirectoryInfo dir1 = new DirectoryInfo(args[2]);
                        DirectoryInfo dir2 = new DirectoryInfo(args[1]);
                        if(CreateSymbolicLink(dir1.FullName, dir2.FullName, SYMBOLIC_LINK_FLAG_DIRECTORY))
                        {
                            Console.WriteLine("Sym link create successfully...");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Sym link wasn't create successfully...");
                            return;
                        } 
                    }
                }

                return;
            }
            else if(args.Length == 2) // hard link
            {
                FileInfo file = new FileInfo(args[0]);
                FileInfo file2 = new FileInfo(args[1]);
                if (CreateHardLink(file2.FullName, file.FullName, IntPtr.Zero))
                {
                    Console.WriteLine("Hard link was created successfully...");
                }
                else
                {
                    Console.WriteLine("Hard link was not created successfully...");
                }

                return;
            }

            Console.WriteLine("Incorrect usage...");
            return;
        }
    }
}
