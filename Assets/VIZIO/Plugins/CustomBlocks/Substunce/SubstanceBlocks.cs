using System;
using System.Collections.Generic;
using System.Linq;
using Antares.Vizio.Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

#if !UNITY_IPHONE && !UNITY_ANDROID

namespace Antares.Vizio.Blocks
{
    [VisualLogicBlockDescription("Used to specify the Substance engine CPU usage.")]
    [VisualLogicBlock("ProcessorUsage", "ProceduralMaterial", ParentName = "ProceduralMaterial Set")]
    public class SubstanceProcessorUsageBlock : LogicBlock
    {
        [Parameter(VariableType.In, typeof(ProceduralProcessorUsage), Name = "Processor usage mode")]
        public Variable cpuMode;

        [Parameter(VariableType.Out, typeof(ProceduralProcessorUsage), Name = "Processor usage mode out")]
        public Variable cpuModeOut;

        [EntryTrigger]
        public void SetValue()
        {
            cpuModeOut.Value = ProceduralMaterial.substanceProcessorUsage = (ProceduralProcessorUsage) cpuMode.Value;
        }

        public override bool GetUseCustomTriggers()
        {
            return true;
        }

        public override void OnInitializeDefaultData()
        {
            if (ProceduralMaterial.isSupported)
                cpuMode.Value = ProceduralProcessorUsage.Half;
            RegisterOutputTrigger("Done");
            AddCustomTrigger();
        }
    }

    [VisualLogicBlockDescription("Checks if the Procedural Materials are supported on the current platform.")]
    [VisualLogicBlock("Is Supported", "ProceduralMaterial", ParentName = "ProceduralMaterial Get")]
    public class SubstanceIsSupportedBlock : LogicBlock
    {
        [Parameter(VariableType.Out, typeof(bool), Name = "IS supported")]
        public Variable isSupported;

        [EntryTrigger]
        public void Check()
        {
            if (ProceduralMaterial.isSupported)
            {
                isSupported.Value = true;
                ActivateTrigger();
                return;
            }

            isSupported.Value = false;
            ActivateTrigger(1);
        }

        public override void OnInitializeDefaultData()
        {
            RegisterOutputTrigger("True");
            RegisterOutputTrigger("False");
        }
    }

    [VisualLogicBlockDescription("Will return an array of all available procedural properties names.", Url = "http://forum.antares-universe.com/get-properties-and-get-property-type-nodes-t496.html")]
    [VisualLogicBlock("Properties", "ProceduralMaterial", ParentName = "ProceduralMaterial Get")]
    public class SubstanceGetPropertiesBlock : LogicBlock
    {
        [Parameter(VariableType.In, typeof(ProceduralMaterial), Name = "Substance")]
        public Variable material;

        [Parameter(VariableType.Out, typeof(string[]), Name = "parameters list")]
        public Variable parameters;

        [EntryTrigger("Get Type")]
        public void Check()
        {
            ProceduralMaterial pm = material.Value as ProceduralMaterial;

            List<string> descriptions = new List<string>();
            foreach (var desc in pm.GetProceduralPropertyDescriptions())
            {
                descriptions.Add(desc.name);
            }

            descriptions.Sort();
            parameters.Value = descriptions.ToArray();

            ActivateTrigger();
            ActivateCustomTriggers();
        }

        public override bool GetUseCustomTriggers()
        {
            return true;
        }

        public override void OnInitializeDefaultData()
        {
            RegisterOutputTrigger("Done");
            AddCustomTrigger();
        }
    }

    [VisualLogicBlockDescription("Return the type of the property of the Procedural Material", Url = "http://forum.antares-universe.com/get-properties-and-get-property-type-nodes-t496.html")]
    [VisualLogicBlock("Property Type", "ProceduralMaterial", ParentName = "ProceduralMaterial Get")]
    public class SubstanceGetPropertyTypeBlock : LogicBlock
    {
        [Parameter(VariableType.In, typeof(ProceduralMaterial), Name = "Substance")]
        public Variable material;

        [Parameter(VariableType.In, typeof(string), Name = "Property name")]
        public Variable name;

        [Parameter(VariableType.Out, typeof(System.Type), Name = "Type")]
        public Variable type;

        [Parameter(VariableType.Out, typeof(string), Name = "Type description")]
        public Variable typeName;

        [EntryTrigger]
        public void Check()
        {
            ProceduralMaterial pm = material.Value as ProceduralMaterial;
            if (pm == null)
            {
                Error();
                return;
            }
            string pr = name.Value as string;
            if(string.IsNullOrEmpty(pr) || !pm.HasProceduralProperty(pr))
            {
                Error();
                return;
            }

            var prop = pm.GetProceduralPropertyDescriptions().SingleOrDefault(t => t.name == pr);
            if (prop != null)
                switch (prop.type)
                {
                    case ProceduralPropertyType.Boolean:
                        type.Value = typeof (bool);
                        typeName.Value = typeof (bool).Name;
                        ActivateTrigger("BOOL");
                        break;
                    case ProceduralPropertyType.Float:
                        type.Value = typeof (float);
                        typeName.Value = "float (single)";
                        ActivateTrigger("FLOAT");
                        break;
                    case ProceduralPropertyType.Vector2:
                        type.Value = typeof (Vector2);
                        typeName.Value = typeof (Vector2).Name;
                        ActivateTrigger("VECTOR2");
                        break;
                    case ProceduralPropertyType.Vector3:
                        type.Value = typeof (Vector3);
                        typeName.Value = typeof (Vector3).Name;
                        ActivateTrigger("VECTOR3");
                        break;
                    case ProceduralPropertyType.Vector4:
                        type.Value = typeof (Vector4);
                        typeName.Value = typeof (Vector4).Name;
                        ActivateTrigger("VECTOR4");
                        break;
                    case ProceduralPropertyType.Color3:
                        type.Value = typeof (Color);
                        typeName.Value = typeof (Color).Name;
                        ActivateTrigger("COLOR RGB");
                        break;
                    case ProceduralPropertyType.Color4:
                        type.Value = typeof (Color);
                        typeName.Value = typeof (Color).Name;
                        ActivateTrigger("COLOR RGBA");
                        break;
                    case ProceduralPropertyType.Enum:
                        type.Value = typeof (Enum);
                        typeName.Value = "Enum";
                        ActivateTrigger("ENUM");
                        break;
                    case ProceduralPropertyType.Texture:
                        type.Value = typeof (Texture);
                        typeName.Value = "Texture";
                        ActivateTrigger("TEXTURE");
                        break;
                    default:
                        Error();
                        return;
                }
            else
            {
                Error();
                return;
            }
        }

        void Error()
        {
            type.Value = typeof (object);
            typeName.Value = "NONE";
            ActivateTrigger();
        }

        public override void OnInitializeDefaultData()
        {
            RegisterOutputTrigger("Not Found");
            RegisterOutputTrigger("BOOL");
            RegisterOutputTrigger("COLOR RGB");
            RegisterOutputTrigger("COLOR RGBA");
            RegisterOutputTrigger("ENUM");
            RegisterOutputTrigger("FLOAT");
            RegisterOutputTrigger("VECTOR2");
            RegisterOutputTrigger("VECTOR3");
            RegisterOutputTrigger("VECTOR4");
            RegisterOutputTrigger("TEXTURE");
        }
    }
}
#endif
