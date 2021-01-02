/*
 * Created by SharpDevelop.
 * User: Ahmed
 * Date: 1/2/2021
 * Time: 6:57 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Collections.Generic;

namespace MSIAfterburner.MAHM
{
	public struct Header
	{
		public int	dwSignature;
		//signature allows applications to verify status of shared memory

		//The signature can be set to:
		//'MAHM'	- hardware monitoring memory is initialized and contains
		//			valid data
		//0xDEAD	- hardware monitoring memory is marked for deallocation and
		//			no longer contain valid data
		//otherwise the memory is not initialized
		public int	dwVersion;
		//header version ((major<<16) + minor)
		//must be set to 0x00020000 for v2.0
		public int	dwHeaderSize;
		//size of header
		public int	dwNumEntries;
		//number of subsequent MAHM_SHARED_MEMORY_ENTRY entries
		public int	dwEntrySize;
		//size of entries in subsequent MAHM_SHARED_MEMORY_ENTRY entries array
		public int time;
		//last polling time

		//WARNING! Force 32-bit time_t usage with #define _USE_32BIT_TIME_T
		//to provide compatibility with VC8.0 and newer compiler versions

		//WARNING! The following fields are valid for v2.0 and newer shared memory layouts only

		public int	dwNumGpuEntries;
		//number of subsequent MAHM_SHARED_MEMORY_GPU_ENTRY entries
		public int	dwGpuEntrySize;
		//size of entries in subsequent MAHM_SHARED_MEMORY_GPU_ENTRY entries array
		
		public DateTime Time()
		{
			return new DateTime(1970, 1, 1).AddSeconds(time).ToLocalTime();
		}
	}
	
	[Flags]
	public enum EntryFlag
	{
		None = 0,
		MAHM_SHARED_MEMORY_ENTRY_FLAG_SHOW_IN_OSD = 0x00000001,
		MAHM_SHARED_MEMORY_ENTRY_FLAG_SHOW_IN_LCD = 0x00000002,
		MAHM_SHARED_MEMORY_ENTRY_FLAG_SHOW_IN_TRAY = 0x00000004
	}
	
	public enum SourceID{
		MONITORING_SOURCE_ID_UNKNOWN = -1,
		MONITORING_SOURCE_ID_GPU_TEMPERATURE = 0x00000000,
		MONITORING_SOURCE_ID_PCB_TEMPERATURE = 0x00000001,
		MONITORING_SOURCE_ID_MEM_TEMPERATURE = 0x00000002,
		MONITORING_SOURCE_ID_VRM_TEMPERATURE = 0x00000003,
		MONITORING_SOURCE_ID_FAN_SPEED = 0x00000010,
		MONITORING_SOURCE_ID_FAN_TACHOMETER = 0x00000011,
		MONITORING_SOURCE_ID_FAN_SPEED2 = 0x00000012,
		MONITORING_SOURCE_ID_FAN_TACHOMETER2 = 0x00000013,
		MONITORING_SOURCE_ID_FAN_SPEED3 = 0x00000014,
		MONITORING_SOURCE_ID_FAN_TACHOMETER3 = 0x00000015,
		MONITORING_SOURCE_ID_CORE_CLOCK = 0x00000020,
		MONITORING_SOURCE_ID_SHADER_CLOCK = 0x00000021,
		MONITORING_SOURCE_ID_MEMORY_CLOCK = 0x00000022,
		MONITORING_SOURCE_ID_GPU_USAGE = 0x00000030,
		MONITORING_SOURCE_ID_MEMORY_USAGE = 0x00000031,
		MONITORING_SOURCE_ID_FB_USAGE = 0x00000032,
		MONITORING_SOURCE_ID_VID_USAGE = 0x00000033,
		MONITORING_SOURCE_ID_BUS_USAGE = 0x00000034,
		MONITORING_SOURCE_ID_GPU_VOLTAGE = 0x00000040,
		MONITORING_SOURCE_ID_AUX_VOLTAGE = 0x00000041,
		MONITORING_SOURCE_ID_MEMORY_VOLTAGE = 0x00000042,
		MONITORING_SOURCE_ID_AUX2_VOLTAGE = 0x00000043,
		MONITORING_SOURCE_ID_FRAMERATE = 0x00000050,
		MONITORING_SOURCE_ID_FRAMETIME = 0x00000051,
		MONITORING_SOURCE_ID_FRAMERATE_MIN = 0x00000052,
		MONITORING_SOURCE_ID_FRAMERATE_AVG = 0x00000053,
		MONITORING_SOURCE_ID_FRAMERATE_MAX = 0x00000054,
		MONITORING_SOURCE_ID_FRAMERATE_1DOT0_PERCENT_LOW = 0x00000055,
		MONITORING_SOURCE_ID_FRAMERATE_0DOT1_PERCENT_LOW = 0x00000056,
		MONITORING_SOURCE_ID_GPU_POWER = 0x00000060,
		MONITORING_SOURCE_ID_GPU_TEMP_LIMIT = 0x00000070,
		MONITORING_SOURCE_ID_GPU_POWER_LIMIT = 0x00000071,
		MONITORING_SOURCE_ID_GPU_VOLTAGE_LIMIT = 0x00000072,
		MONITORING_SOURCE_ID_GPU_UTIL_LIMIT = 0x00000074,
		MONITORING_SOURCE_ID_GPU_SLI_SYNC_LIMIT = 0x00000075,
		MONITORING_SOURCE_ID_CPU_TEMPERATURE = 0x00000080,
		MONITORING_SOURCE_ID_CPU_USAGE = 0x00000090,
		MONITORING_SOURCE_ID_RAM_USAGE = 0x00000091,
		MONITORING_SOURCE_ID_PAGEFILE_USAGE = 0x00000092,
		MONITORING_SOURCE_ID_CPU_CLOCK = 0x000000A0,
		MONITORING_SOURCE_ID_GPU_TEMPERATURE2 = 0x000000B0,
		MONITORING_SOURCE_ID_PCB_TEMPERATURE2 = 0x000000B1,
		MONITORING_SOURCE_ID_MEM_TEMPERATURE2 = 0x000000B2,
		MONITORING_SOURCE_ID_VRM_TEMPERATURE2 = 0x000000B3,
		MONITORING_SOURCE_ID_GPU_TEMPERATURE3 = 0x000000C0,
		MONITORING_SOURCE_ID_PCB_TEMPERATURE3 = 0x000000C1,
		MONITORING_SOURCE_ID_MEM_TEMPERATURE3 = 0x000000C2,
		MONITORING_SOURCE_ID_VRM_TEMPERATURE3 = 0x000000C3,
		MONITORING_SOURCE_ID_GPU_TEMPERATURE4 = 0x000000D0,
		MONITORING_SOURCE_ID_PCB_TEMPERATURE4 = 0x000000D1,
		MONITORING_SOURCE_ID_MEM_TEMPERATURE4 = 0x000000D2,
		MONITORING_SOURCE_ID_VRM_TEMPERATURE4 = 0x000000D3,
		MONITORING_SOURCE_ID_GPU_TEMPERATURE5 = 0x000000E0,
		MONITORING_SOURCE_ID_PCB_TEMPERATURE5 = 0x000000E1,
		MONITORING_SOURCE_ID_MEM_TEMPERATURE5 = 0x000000E2,
		MONITORING_SOURCE_ID_VRM_TEMPERATURE5 = 0x000000E3,
		MONITORING_SOURCE_ID_PLUGIN_GPU = 0x000000F0,
		MONITORING_SOURCE_ID_PLUGIN_CPU = 0x000000F1,
		MONITORING_SOURCE_ID_PLUGIN_MOBO = 0x000000F2,
		MONITORING_SOURCE_ID_PLUGIN_RAM = 0x000000F3,
		MONITORING_SOURCE_ID_PLUGIN_HDD = 0x000000F4,
		MONITORING_SOURCE_ID_PLUGIN_NET = 0x000000F5,
		MONITORING_SOURCE_ID_PLUGIN_PSU = 0x000000F6,
		MONITORING_SOURCE_ID_PLUGIN_UPS = 0x000000F7,
		MONITORING_SOURCE_ID_PLUGIN_MISC = 0x000000FF,
		MONITORING_SOURCE_ID_CPU_POWER = 0x00000100
	}
	
	public struct Entry
	{

		public string	szSrcName;
		//data source name (e.g. "Core clock")
		public string	szSrcUnits;
		//data source units (e.g. "MHz")

		public string	szLocalizedSrcName;
		//localized data source name (e.g. "„астота ¤дра" for Russian GUI)
		public string	szLocalizedSrcUnits;
		//localized data source units (e.g. "ћ√ц" for Russian GUI)

		public string	szRecommendedFormat;
		//recommended output format (e.g. "%.3f" for "Core voltage" data source)

		public float	data;
		//last polled data (e.g. 500MHz)
		//(this field can be set to FLT_MAX if data is not available at
		//the moment)
		public float	minLimit;
		//minimum limit for graphs (e.g. 0MHz)
		public float	maxLimit;
		//maximum limit for graphs (e.g. 2000MHz)

		public EntryFlag	dwFlags;
		//bitmask containing combination of MAHM_SHARED_MEMORY_ENTRY_FLAG_...

		//WARNING! The following fields are valid for v2.0 and newer shared memory layouts only

		public int	dwGpu;
		//data source GPU index (zero based) or 0xFFFFFFFF for global data sources (e.g. Framerate)
		public SourceID	dwSrcId;
		//data source ID
		
		public override string ToString()
		{
			return string.Format("{0} = {2}{1}", szSrcName, szSrcUnits, data);
		}

	}
	
	public struct GPUEntry
	{
		public string	szGpuId;
		//GPU identifier represented in VEN_%04X&DEV_%04X&SUBSYS_%08X&REV_%02X&BUS_%d&DEV_%d&FN_%d format
		//(e.g. VEN_10DE&DEV_0A20&SUBSYS_071510DE&BUS_1&DEV_0&FN_0)

		public string	szFamily;
		//GPU family (e.g. "GT216")
		//can be empty if data is not available
		public string	szDevice;
		//display device description (e.g. "GeForce GT 220")
		//can be empty if data is not available
		public string	szDriver;
		//display driver description (e.g. "6.14.11.9621, ForceWare 196.21")
		//can be empty if data is not available
		public string	szBIOS;
		//BIOS version (e.g. 70.16.24.00.00)
		//can be empty if data is not available
		public int	dwMemAmount;
		//on-board memory amount in KB (e.g. 1048576)
		//can be empty if data is not available

	}
	
	public struct Data
	{
		public Header header;
		public Entry[] entries;
		public GPUEntry[] gpu_entries;
	}
	
	public class Reader
	{
		const int MAX_PATH = 260;
		private MemoryMappedFile memfile;
		private BinaryReader br;
		
		public void Connect()
		{
			memfile = MemoryMappedFile.OpenExisting("MAHMSharedMemory");
			
			//check memfile is still valid
			//
			//start collection data
			
			br = new BinaryReader(memfile.CreateViewStream());
		}
		
		public void DisConnect()
		{
			br.BaseStream.Close();
			memfile.Dispose();
		}
		
		string ReadString()
		{
			byte[] c = br.ReadBytes(MAX_PATH);
			return System.Text.Encoding.UTF8.GetString(c).TrimEnd('\0');
		}
		
		public Data ReadData()
		{
			Data data = new Data();
			
			data.header.dwSignature = br.ReadInt32();
			data.header.dwVersion = br.ReadInt32();
			data.header.dwHeaderSize = br.ReadInt32();
			data.header.dwNumEntries = br.ReadInt32();
			data.header.dwEntrySize = br.ReadInt32();
			data.header.time = br.ReadInt32();
			data.header.dwNumGpuEntries = br.ReadInt32();
			data.header.dwGpuEntrySize = br.ReadInt32();
			
			// All entries 		then
			// All gpu_entries
			// Ignoring sizes but reading whole structures
			
			List<Entry> entries = new List<Entry>();
			for (int i = 0; i < data.header.dwNumEntries; i++) {
				Entry entry = new Entry();
				entry.szSrcName = ReadString();
				entry.szSrcUnits = ReadString();
				entry.szLocalizedSrcName = ReadString();
				entry.szLocalizedSrcUnits = ReadString();
				entry.szRecommendedFormat = ReadString();
				entry.data = br.ReadSingle();
				entry.minLimit = br.ReadSingle();
				entry.maxLimit = br.ReadSingle();
				entry.dwFlags = (EntryFlag)br.ReadInt32();
				entry.dwGpu = br.ReadInt32();
				entry.dwSrcId = (SourceID)br.ReadInt32();
				
				entries.Add(entry);
			}
			data.entries = entries.ToArray();
			entries.Clear(); // free ram
			
			List<GPUEntry> gpu_entries = new List<GPUEntry>();
			
			for (int i = 0; i < data.header.dwNumGpuEntries; i++) {
				GPUEntry entry = new GPUEntry();
				
				entry.szGpuId = ReadString();
				entry.szFamily = ReadString();
				entry.szDevice = ReadString();
				entry.szDriver = ReadString();
				entry.szBIOS = ReadString();
				entry.dwMemAmount = br.ReadInt32();
				
				gpu_entries.Add(entry);
			}
			
			data.gpu_entries = gpu_entries.ToArray();
			
			return data;
		}
	}
}
