using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testproj
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ScenarioInstanceMap
    {

        private ScenarioInstanceMapArgument[] deploymentField;

        private ScenarioInstanceMapMapping[] mappingField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Argument", IsNullable = false)]
        public ScenarioInstanceMapArgument[] Deployment
        {
            get
            {
                return this.deploymentField;
            }
            set
            {
                this.deploymentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Mapping")]
        public ScenarioInstanceMapMapping[] Mapping
        {
            get
            {
                return this.mappingField;
            }
            set
            {
                this.mappingField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ScenarioInstanceMapArgument
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ScenarioInstanceMapMapping
    {

        private string instancesField;

        private ScenarioInstanceMapMappingWorkloadScenario[] workloadScenariosField;

        /// <remarks/>
        public string Instances
        {
            get
            {
                return this.instancesField;
            }
            set
            {
                this.instancesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("WorkloadScenario", IsNullable = false)]
        public ScenarioInstanceMapMappingWorkloadScenario[] WorkloadScenarios
        {
            get
            {
                return this.workloadScenariosField;
            }
            set
            {
                this.workloadScenariosField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ScenarioInstanceMapMappingWorkloadScenario
    {

        private string typeField;

        private string nameField;

        private ScenarioInstanceMapMappingWorkloadScenarioArgument[] argumentsField;

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Argument", IsNullable = false)]
        public ScenarioInstanceMapMappingWorkloadScenarioArgument[] Arguments
        {
            get
            {
                return this.argumentsField;
            }
            set
            {
                this.argumentsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ScenarioInstanceMapMappingWorkloadScenarioArgument
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

}
