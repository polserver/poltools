using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System
{
	public unsafe abstract class ProcessStream : Stream
	{
		private const int ProcessAllAccess = 0x1F0FFF;

		[DllImport( "Kernel32" )]
		private static extern IntPtr OpenProcess( int desiredAccess, int inheritHandle, IntPtr processID );

		[DllImport( "Kernel32" )]
		private static extern int CloseHandle( IntPtr handle );

		[DllImport( "Kernel32" )]
		private static extern int ReadProcessMemory( IntPtr process, int baseAddress, void *buffer, int size, ref int op );

		[DllImport( "Kernel32" )]
		private static extern int WriteProcessMemory( IntPtr process, int baseAddress, void *buffer, int size, int nullMe );

		protected bool m_Open;
		protected IntPtr m_Process;

		protected int m_Position;

		public abstract IntPtr ProcessID{ get; }

		public ProcessStream()
		{
		}

		public virtual bool BeginAccess()
		{
			if ( m_Open )
				return false;

			m_Process = OpenProcess( ProcessAllAccess, 0, ProcessID );
			m_Open = true;

			return true;
		}

		public virtual void EndAccess()
		{
			if ( !m_Open )
				return;

			CloseHandle( m_Process );
			m_Open = false;
		}

		public override void Flush()
		{
		}

		public override int Read( byte[] buffer, int offset, int count )
		{
			bool end = !BeginAccess();

			int res = 0;

			fixed ( byte *p = buffer )
				ReadProcessMemory( m_Process, m_Position, p + offset, count, ref res );

			m_Position += count;

			if ( end )
				EndAccess();

			return res;
		}

		public override void Write( byte[] buffer, int offset, int count )
		{
			bool end = !BeginAccess();

			fixed ( byte *p = buffer )
				WriteProcessMemory( m_Process, m_Position, p + offset, count, 0 );

			m_Position += count;

			if ( end )
				EndAccess();
		}

		public override bool CanRead{ get{ return true; } }
		public override bool CanWrite{ get{ return true; } }
		public override bool CanSeek{ get{ return true; } }

		public override long Length{ get{ throw new NotSupportedException(); } }
		public override long Position{ get{ return m_Position; } set{ m_Position = (int)value; } }

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override long Seek( long offset, SeekOrigin origin )
		{
			switch ( origin )
			{
				case SeekOrigin.Begin: m_Position = (int)offset; break;
				case SeekOrigin.Current: m_Position += (int)offset; break;
				case SeekOrigin.End: throw new NotSupportedException();
			}

			return m_Position;
		}
	}
}