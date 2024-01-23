//
//  Stfu — Sam’s Tiny Framework Utilities
//
//  Copyright © 2013–2023 Sam Hocevar <sam@hocevar.net>
//
//  This library is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

using System;
using System.IO;
using System.Text;

namespace Stfu
{
    public class AtomicFileWriter : TextWriter
    {
        public AtomicFileWriter(string path)
        {
            Initialize(path, false);
            m_stream = new StreamWriter(TemporaryPath);
        }

        public AtomicFileWriter(string path, bool append)
        {
            Initialize(path, append);
            m_stream = new StreamWriter(TemporaryPath, append);
        }

        public AtomicFileWriter(string path, bool append, Encoding encoding)
        {
            Initialize(path, append);
            m_stream = new StreamWriter(TemporaryPath, append, encoding);
        }

        public AtomicFileWriter(string path, bool append, Encoding encoding, int bufferSize)
        {
            Initialize(path, append);
            m_stream = new StreamWriter(TemporaryPath, append, encoding, bufferSize);
        }

        /// <summary>
        /// This is the only method that actually needs implementing, all the others rely on this
        /// </summary>
        /// <param name="c"></param>
        public override void Write(char c)
            => m_stream.Write(c);

        /// <summary>
        /// This override is not necessary but probably helps a bit with performance
        /// </summary>
        /// <param name="s"></param>
        public override void Write(string s)
            => m_stream.Write(s);

        public override Encoding Encoding
            => m_stream.Encoding;

        public void Commit()
        {
            if (m_closed)
                throw new InvalidOperationException();

            m_stream.Close();
            m_closed = true;

#if NETCOREAPP || NETSTANDARD
            File.Move(TemporaryPath, Path, overwrite: true);
#else
            // Not very atomic unfortunately, but this is how MSFT does it in the
            // File.Move() documentation for .NET 7.0.
            File.Delete(Path);
            File.Move(TemporaryPath, Path);
#endif
        }

        // Note from the TextWriter.Close() documentation:
        // “In derived classes, do not override the Close method. Instead, override the
        // TextWriter.Dispose(Boolean) method to add code for releasing resources.”
        protected override void Dispose(bool disposing)
        {
            // Note from the TextWriter.Dispose() documentation:
            // ”Dispose(Boolean) can be called multiple times by other objects. When
            // overriding this method, be careful not to reference objects that have
            // been previously disposed of in an earlier call to Dispose.”
            if (!m_disposed)
            {
                m_stream.Dispose();
                m_disposed = true;

                try
                {
                    File.Delete(TemporaryPath);
                }
                catch { }
            }
        }

        public string Path { get; private set; }

        public string TemporaryPath { get; private set; }

        private void Initialize(string path, bool append)
        {
            Path = path;
            TemporaryPath = $"{path}~";

            // Best way to detect path and permission errors early is to try to append
            // to the destination file and immediately close it. Otherwise these issues
            // are not known until the stream is closed.
            if (File.Exists(Path))
            {
                using (var _ = new StreamWriter(Path, append: true))
                {
                }
            }

            // If opened in append mode, first copy the original file to the temporary
            // file location. We will append to that new file.
            if (append && File.Exists(Path))
            {
                File.Copy(Path, TemporaryPath);
            }
        }

        private StreamWriter m_stream;

        private bool m_closed;
        private bool m_disposed;
    }
}
