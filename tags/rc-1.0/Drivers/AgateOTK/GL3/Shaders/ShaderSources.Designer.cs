﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AgateOTK.GL3.Shaders {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ShaderSources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ShaderSources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AgateOTK.GL3.Shaders.ShaderSources", typeof(ShaderSources).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 140
        ///
        ///uniform sampler2D texture;
        ///
        ///in vec4 colorVal;
        ///in vec2 texCoordVal;
        ///
        ///out vec4 color;
        ///
        ///void main()
        ///{
        ///	color = texture2D(texture, texCoordVal);
        ///	color = color * colorVal;
        ///}
        ///.
        /// </summary>
        internal static string Basic2D_pixel {
            get {
                return ResourceManager.GetString("Basic2D_pixel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 140
        ///
        ///in vec3 position;
        ///in vec4 color;
        ///in vec2 texCoord;
        ///
        ///uniform mat4x4 transform;
        ///
        ///out vec4 colorVal;
        ///out vec2 texCoordVal;
        ///
        ///void main()
        ///{
        ///	vec4 pos = vec4(position, 1);
        ///	
        ///	colorVal = color;
        ///	texCoordVal = texCoord;
        ///	
        ///    gl_Position = transform * pos;
        ///}
        ///.
        /// </summary>
        internal static string Basic2D_vert {
            get {
                return ResourceManager.GetString("Basic2D_vert", resourceCulture);
            }
        }
    }
}
