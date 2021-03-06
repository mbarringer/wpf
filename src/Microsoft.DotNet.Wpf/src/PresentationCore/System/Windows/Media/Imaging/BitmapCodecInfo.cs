// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using MS.Internal;
using MS.Win32.PresentationCore;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Windows.Media.Imaging;
using System.Text;
using MS.Internal.PresentationCore;                        // SecurityHelper

namespace System.Windows.Media.Imaging
{
    #region BitmapCodecInfo

    /// <summary>
    /// Codec info for a given Encoder/Decoder
    /// </summary>
    public abstract class BitmapCodecInfo
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        protected BitmapCodecInfo()
        {
        }

        /// <summary>
        /// Internal Constructor
        /// </summary>
        internal BitmapCodecInfo(SafeMILHandle codecInfoHandle)
        {
            Debug.Assert(codecInfoHandle != null);
            _isBuiltIn = true;
            _codecInfoHandle = codecInfoHandle;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Container format
        /// </summary>
        /// <remarks>
        ///     Callers must have RegistryPermission(PermissionState.Unrestricted) to call this API.
        /// </remarks>
        /// <SecurityNote>
        /// Critical - calls unamanged code to retrieve data
        /// PublicOK - Demands registry permissions
        /// </SecurityNote>
        public virtual Guid ContainerFormat
        {
            [SecurityCritical]
            get
            {
                SecurityHelper.DemandRegistryPermission();

                EnsureBuiltIn();

                Guid containerFormat;

                HRESULT.Check(UnsafeNativeMethods.WICBitmapCodecInfo.GetContainerFormat(
                    _codecInfoHandle,
                    out containerFormat
                    ));

                return containerFormat;
            }
        }

        /// <summary>
        /// Author
        /// </summary>
        /// <remarks>
        ///     Callers must have RegistryPermission(PermissionState.Unrestricted) to call this API.
        /// </remarks>
        /// <SecurityNote>
        /// Critical - calls unamanged code to retrieve data
        /// PublicOK - Demands registry permissions
        /// </SecurityNote>
        public virtual string Author
        {
            [SecurityCritical]
            get
            {
                SecurityHelper.DemandRegistryPermission();

                EnsureBuiltIn();

                StringBuilder author = null;
                UInt32 length = 0;

                // Find the length of the string needed
                HRESULT.Check(UnsafeNativeMethods.WICComponentInfo.GetAuthor(
                    _codecInfoHandle,
                    0,
                    author,
                    out length
                    ));

                Debug.Assert(length >= 0);

                // get the string back
                if (length > 0)
                {
                    author = new StringBuilder((int)length);

                    HRESULT.Check(UnsafeNativeMethods.WICComponentInfo.GetAuthor(
                        _codecInfoHandle,
                        length,
                        author,
                        out length
                        ));
                }

                if (author != null)
                    return author.ToString();
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        /// <remarks>
        ///     Callers must have RegistryPermission(PermissionState.Unrestricted) to call this API.
        /// </remarks>
        /// <SecurityNote>
        /// Critical - calls unamanged code to retrieve data
        /// PublicOK - Demands registry permissions
        /// </SecurityNote>
        public virtual System.Version Version
        {
            [SecurityCritical]
            get
            {
                SecurityHelper.DemandRegistryPermission();

                EnsureBuiltIn();

                StringBuilder version = null;
                UInt32 length = 0;

                // Find the length of the string needed
                HRESULT.Check(UnsafeNativeMethods.WICComponentInfo.GetVersion(
                    _codecInfoHandle,
                    0,
                    version,
                    out length
                    ));

                Debug.Assert(length >= 0);

                // get the string back
                if (length > 0)
                {
                    version = new StringBuilder((int)length);

                    HRESULT.Check(UnsafeNativeMethods.WICComponentInfo.GetVersion(
                        _codecInfoHandle,
                        length,
                        version,
                        out length
                        ));
                }

                if (version != null)
                    return new Version(version.ToString());
                else
                    return new Version();
            }
        }

        /// <summary>
        /// Spec Version
        /// </summary>
        /// <remarks>
        ///     Callers must have RegistryPermission(PermissionState.Unrestricted) to call this API.
        /// </remarks>
        /// <SecurityNote>
        /// Critical - calls unamanged code to retrieve data
        /// PublicOK - Demands registry permissions
        /// </SecurityNote>
        public virtual Version SpecificationVersion
        {
            [SecurityCritical]
            get
            {
                SecurityHelper.DemandRegistryPermission();

                EnsureBuiltIn();

                StringBuilder specVersion = null;
                UInt32 length = 0;

                // Find the length of the string needed
                HRESULT.Check(UnsafeNativeMethods.WICComponentInfo.GetSpecVersion(
                    _codecInfoHandle,
                    0,
                    specVersion,
                    out length
                    ));

                Debug.Assert(length >= 0);

                // get the string back
                if (length > 0)
                {
                    specVersion = new StringBuilder((int)length);

                    HRESULT.Check(UnsafeNativeMethods.WICComponentInfo.GetSpecVersion(
                        _codecInfoHandle,
                        length,
                        specVersion,
                        out length
                        ));
                }

                if (specVersion != null)
                    return new Version(specVersion.ToString());
                else
                    return new Version();
            }
        }

        /// <summary>
        /// Friendly Name
        /// </summary>
        /// <remarks>
        ///     Callers must have RegistryPermission(PermissionState.Unrestricted) to call this API.
        /// </remarks>
        /// <SecurityNote>
        /// Critical - calls unamanged code to retrieve data
        /// PublicOK - Demands registry permissions
        /// </SecurityNote>
        public virtual string FriendlyName
        {
            [SecurityCritical]
            get
            {
                SecurityHelper.DemandRegistryPermission();

                EnsureBuiltIn();

                StringBuilder friendlyName = null;
                UInt32 length = 0;

                // Find the length of the string needed
                HRESULT.Check(UnsafeNativeMethods.WICComponentInfo.GetFriendlyName(
                    _codecInfoHandle,
                    0,
                    friendlyName,
                    out length
                    ));

                Debug.Assert(length >= 0);

                // get the string back
                if (length > 0)
                {
                    friendlyName = new StringBuilder((int)length);

                    HRESULT.Check(UnsafeNativeMethods.WICComponentInfo.GetFriendlyName(
                        _codecInfoHandle,
                        length,
                        friendlyName,
                        out length
                        ));
                }

                if (friendlyName != null)
                    return friendlyName.ToString();
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// Device Manufacturer
        /// </summary>
        /// <remarks>
        ///     Callers must have RegistryPermission(PermissionState.Unrestricted) to call this API.
        /// </remarks>
        /// <SecurityNote>
        /// Critical - calls unamanged code to retrieve data
        /// PublicOK - Demands registry permissions
        /// </SecurityNote>
        public virtual string DeviceManufacturer
        {
            [SecurityCritical]
            get
            {
                SecurityHelper.DemandRegistryPermission();

                EnsureBuiltIn();

                StringBuilder deviceManufacturer = null;
                UInt32 length = 0;

                // Find the length of the string needed
                HRESULT.Check(UnsafeNativeMethods.WICBitmapCodecInfo.GetDeviceManufacturer(
                    _codecInfoHandle,
                    0,
                    deviceManufacturer,
                    out length
                    ));

                Debug.Assert(length >= 0);

                // get the string back
                if (length > 0)
                {
                    deviceManufacturer = new StringBuilder((int)length);

                    HRESULT.Check(UnsafeNativeMethods.WICBitmapCodecInfo.GetDeviceManufacturer(
                        _codecInfoHandle,
                        length,
                        deviceManufacturer,
                        out length
                        ));
                }

                if (deviceManufacturer != null)
                    return deviceManufacturer.ToString();
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// Device Models
        /// </summary>
        /// <remarks>
        ///     Callers must have RegistryPermission(PermissionState.Unrestricted) to call this API.
        /// </remarks>
        /// <SecurityNote>
        /// Critical - calls unamanged code to retrieve data
        /// PublicOK - Demands registry permissions
        /// </SecurityNote>
        public virtual string DeviceModels
        {
            [SecurityCritical]
            get
            {
                SecurityHelper.DemandRegistryPermission();

                EnsureBuiltIn();

                StringBuilder deviceModels = null;
                UInt32 length = 0;

                // Find the length of the string needed
                HRESULT.Check(UnsafeNativeMethods.WICBitmapCodecInfo.GetDeviceModels(
                    _codecInfoHandle,
                    0,
                    deviceModels,
                    out length
                    ));

                Debug.Assert(length >= 0);

                // get the string back
                if (length > 0)
                {
                    deviceModels = new StringBuilder((int)length);

                    HRESULT.Check(UnsafeNativeMethods.WICBitmapCodecInfo.GetDeviceModels(
                        _codecInfoHandle,
                        length,
                        deviceModels,
                        out length
                        ));
                }

                if (deviceModels != null)
                    return deviceModels.ToString();
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// Mime types
        /// </summary>
        /// <remarks>
        ///     Callers must have RegistryPermission(PermissionState.Unrestricted) to call this API.
        /// </remarks>
        /// <SecurityNote>
        /// Critical - calls unamanged code to retrieve data
        /// PublicOK - Demands registry permissions
        /// </SecurityNote>
        public virtual string MimeTypes
        {
            [SecurityCritical]
            get
            {
                SecurityHelper.DemandRegistryPermission();

                EnsureBuiltIn();

                StringBuilder mimeTypes = null;
                UInt32 length = 0;

                // Find the length of the string needed
                HRESULT.Check(UnsafeNativeMethods.WICBitmapCodecInfo.GetMimeTypes(
                    _codecInfoHandle,
                    0,
                    mimeTypes,
                    out length
                    ));

                Debug.Assert(length >= 0);

                // get the string back
                if (length > 0)
                {
                    mimeTypes = new StringBuilder((int)length);

                    HRESULT.Check(UnsafeNativeMethods.WICBitmapCodecInfo.GetMimeTypes(
                        _codecInfoHandle,
                        length,
                        mimeTypes,
                        out length
                        ));
                }

                if (mimeTypes != null)
                    return mimeTypes.ToString();
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// File extensions
        /// </summary>
        /// <remarks>
        ///     Callers must have RegistryPermission(PermissionState.Unrestricted) to call this API.
        /// </remarks>
        /// <SecurityNote>
        /// Critical - calls unamanged code to retrieve data
        /// PublicOK - Demands registry permissions
        /// </SecurityNote>
        public virtual string FileExtensions
        {
            [SecurityCritical]
            get
            {
                SecurityHelper.DemandRegistryPermission();

                EnsureBuiltIn();

                StringBuilder fileExtensions = null;
                UInt32 length = 0;

                // Find the length of the string needed
                HRESULT.Check(UnsafeNativeMethods.WICBitmapCodecInfo.GetFileExtensions(
                    _codecInfoHandle,
                    0,
                    fileExtensions,
                    out length
                    ));

                Debug.Assert(length >= 0);

                // get the string back
                if (length > 0)
                {
                    fileExtensions = new StringBuilder((int)length);

                    HRESULT.Check(UnsafeNativeMethods.WICBitmapCodecInfo.GetFileExtensions(
                        _codecInfoHandle,
                        length,
                        fileExtensions,
                        out length
                        ));
                }

                if (fileExtensions != null)
                    return fileExtensions.ToString();
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// Does Support Animation
        /// </summary>
        /// <remarks>
        ///     Callers must have RegistryPermission(PermissionState.Unrestricted) to call this API.
        /// </remarks>
        /// <SecurityNote>
        /// Critical - calls unamanged code to retrieve data
        /// PublicOK - Demands registry permissions
        /// </SecurityNote>
        public virtual bool SupportsAnimation
        {
            [SecurityCritical]
            get
            {
                SecurityHelper.DemandRegistryPermission();

                EnsureBuiltIn();

                bool supportsAnimation;

                HRESULT.Check(UnsafeNativeMethods.WICBitmapCodecInfo.DoesSupportAnimation(
                    _codecInfoHandle,
                    out supportsAnimation
                    ));

                return supportsAnimation;
            }
        }

        /// <summary>
        /// Does Support Lossless
        /// </summary>
        /// <remarks>
        ///     Callers must have RegistryPermission(PermissionState.Unrestricted) to call this API.
        /// </remarks>
        /// <SecurityNote>
        /// Critical - calls unamanged code to retrieve data
        /// PublicOK - Demands registry permissions
        /// </SecurityNote>
        public virtual bool SupportsLossless
        {
            [SecurityCritical]
            get
            {
                SecurityHelper.DemandRegistryPermission();

                EnsureBuiltIn();

                bool supportsLossless;

                HRESULT.Check(UnsafeNativeMethods.WICBitmapCodecInfo.DoesSupportLossless(
                    _codecInfoHandle,
                    out supportsLossless
                    ));

                return supportsLossless;
            }
        }

        /// <summary>
        /// Does Support Multiple Frames
        /// </summary>
        /// <remarks>
        ///     Callers must have RegistryPermission(PermissionState.Unrestricted) to call this API.
        /// </remarks>
        /// <SecurityNote>
        /// Critical - calls unamanged code to retrieve data
        /// PublicOK - Demands registry permissions
        /// </SecurityNote>
        public virtual bool SupportsMultipleFrames
        {
            [SecurityCritical]
            get
            {
                SecurityHelper.DemandRegistryPermission();

                EnsureBuiltIn();

                bool supportsMultiFrame;

                HRESULT.Check(UnsafeNativeMethods.WICBitmapCodecInfo.DoesSupportMultiframe(
                    _codecInfoHandle,
                    out supportsMultiFrame
                    ));

                return supportsMultiFrame;
            }
        }

        #endregion

        #region Private Methods

        private void EnsureBuiltIn()
        {
            if (!_isBuiltIn)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Data Members

        /// is this a built in codec info?
        private bool _isBuiltIn;

        /// Codec info handle
        SafeMILHandle _codecInfoHandle;

        #endregion
    }

    #endregion
}
