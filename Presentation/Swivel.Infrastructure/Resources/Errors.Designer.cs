﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Swivel.Infrastructure.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Errors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Errors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Swivel.Infrastructure.Resources.Errors", typeof(Errors).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bad request.
        /// </summary>
        public static string BAD_REQUEST_ERROR_MESSAGE {
            get {
                return ResourceManager.GetString("BAD_REQUEST_ERROR_MESSAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal server error.
        /// </summary>
        public static string INTERNAL_SERVER_ERROR_MESSAGE {
            get {
                return ResourceManager.GetString("INTERNAL_SERVER_ERROR_MESSAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Item(s) filled incorrect.
        /// </summary>
        public static string MODELSTATE_ERROR_MESSAGE {
            get {
                return ResourceManager.GetString("MODELSTATE_ERROR_MESSAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your request not found.
        /// </summary>
        public static string NOT_FOUND_ERROR_MESSAGE {
            get {
                return ResourceManager.GetString("NOT_FOUND_ERROR_MESSAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your operation is not valid.
        ///only &apos;replace&apos; operation is allowed..
        /// </summary>
        public static string PATCH_OPERATION_ERROR_MESSAGE {
            get {
                return ResourceManager.GetString("PATCH_OPERATION_ERROR_MESSAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please make sure that the content type should be &apos;application/json-patch+json&apos;
        ///otherwise Web API won&apos;t invoke the JsonPatch media formatter..
        /// </summary>
        public static string PATCH_REQUEST_ERROR_MESSAGE {
            get {
                return ResourceManager.GetString("PATCH_REQUEST_ERROR_MESSAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Record not available.
        /// </summary>
        public static string RECORD_NOT_AVAILABLE {
            get {
                return ResourceManager.GetString("RECORD_NOT_AVAILABLE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are not authorized.
        /// </summary>
        public static string UNAUTHORIZED_ERROR_MESSAGE {
            get {
                return ResourceManager.GetString("UNAUTHORIZED_ERROR_MESSAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User not found.
        /// </summary>
        public static string USER_NOT_FOUND {
            get {
                return ResourceManager.GetString("USER_NOT_FOUND", resourceCulture);
            }
        }
    }
}
