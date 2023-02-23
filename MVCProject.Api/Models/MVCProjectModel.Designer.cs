﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
namespace MVCProject.Api.Models
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class MVCProjectEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new MVCProjectEntities object using the connection string found in the 'MVCProjectEntities' section of the application configuration file.
        /// </summary>
        public MVCProjectEntities() : base("name=MVCProjectEntities", "MVCProjectEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new MVCProjectEntities object.
        /// </summary>
        public MVCProjectEntities(string connectionString) : base(connectionString, "MVCProjectEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new MVCProjectEntities object.
        /// </summary>
        public MVCProjectEntities(EntityConnection connection) : base(connection, "MVCProjectEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Designation> Designations
        {
            get
            {
                if ((_Designations == null))
                {
                    _Designations = base.CreateObjectSet<Designation>("Designations");
                }
                return _Designations;
            }
        }
        private ObjectSet<Designation> _Designations;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<TblDepartment> TblDepartments
        {
            get
            {
                if ((_TblDepartments == null))
                {
                    _TblDepartments = base.CreateObjectSet<TblDepartment>("TblDepartments");
                }
                return _TblDepartments;
            }
        }
        private ObjectSet<TblDepartment> _TblDepartments;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<TblEmployee> TblEmployee
        {
            get
            {
                if ((_TblEmployee == null))
                {
                    _TblEmployee = base.CreateObjectSet<TblEmployee>("TblEmployee");
                }
                return _TblEmployee;
            }
        }
        private ObjectSet<TblEmployee> _TblEmployee;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Designations EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToDesignations(Designation designation)
        {
            base.AddObject("Designations", designation);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the TblDepartments EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToTblDepartments(TblDepartment tblDepartment)
        {
            base.AddObject("TblDepartments", tblDepartment);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the TblEmployee EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToTblEmployee(TblEmployee tblEmployee)
        {
            base.AddObject("TblEmployee", tblEmployee);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="MVCProjectModel", Name="Designation")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Designation : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Designation object.
        /// </summary>
        /// <param name="designationId">Initial value of the DesignationId property.</param>
        public static Designation CreateDesignation(global::System.Int32 designationId)
        {
            Designation designation = new Designation();
            designation.DesignationId = designationId;
            return designation;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 DesignationId
        {
            get
            {
                return _DesignationId;
            }
            set
            {
                if (_DesignationId != value)
                {
                    OnDesignationIdChanging(value);
                    ReportPropertyChanging("DesignationId");
                    _DesignationId = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("DesignationId");
                    OnDesignationIdChanged();
                }
            }
        }
        private global::System.Int32 _DesignationId;
        partial void OnDesignationIdChanging(global::System.Int32 value);
        partial void OnDesignationIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String DesignationName
        {
            get
            {
                return _DesignationName;
            }
            set
            {
                OnDesignationNameChanging(value);
                ReportPropertyChanging("DesignationName");
                _DesignationName = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("DesignationName");
                OnDesignationNameChanged();
            }
        }
        private global::System.String _DesignationName;
        partial void OnDesignationNameChanging(global::System.String value);
        partial void OnDesignationNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                OnIsActiveChanging(value);
                ReportPropertyChanging("IsActive");
                _IsActive = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("IsActive");
                OnIsActiveChanged();
            }
        }
        private Nullable<global::System.Boolean> _IsActive;
        partial void OnIsActiveChanging(Nullable<global::System.Boolean> value);
        partial void OnIsActiveChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Remarks
        {
            get
            {
                return _Remarks;
            }
            set
            {
                OnRemarksChanging(value);
                ReportPropertyChanging("Remarks");
                _Remarks = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Remarks");
                OnRemarksChanged();
            }
        }
        private global::System.String _Remarks;
        partial void OnRemarksChanging(global::System.String value);
        partial void OnRemarksChanged();

        #endregion

    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="MVCProjectModel", Name="TblDepartment")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class TblDepartment : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new TblDepartment object.
        /// </summary>
        /// <param name="departmentId">Initial value of the DepartmentId property.</param>
        /// <param name="departmentName">Initial value of the DepartmentName property.</param>
        public static TblDepartment CreateTblDepartment(global::System.Int32 departmentId, global::System.String departmentName)
        {
            TblDepartment tblDepartment = new TblDepartment();
            tblDepartment.DepartmentId = departmentId;
            tblDepartment.DepartmentName = departmentName;
            return tblDepartment;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 DepartmentId
        {
            get
            {
                return _DepartmentId;
            }
            set
            {
                if (_DepartmentId != value)
                {
                    OnDepartmentIdChanging(value);
                    ReportPropertyChanging("DepartmentId");
                    _DepartmentId = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("DepartmentId");
                    OnDepartmentIdChanged();
                }
            }
        }
        private global::System.Int32 _DepartmentId;
        partial void OnDepartmentIdChanging(global::System.Int32 value);
        partial void OnDepartmentIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String DepartmentName
        {
            get
            {
                return _DepartmentName;
            }
            set
            {
                OnDepartmentNameChanging(value);
                ReportPropertyChanging("DepartmentName");
                _DepartmentName = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("DepartmentName");
                OnDepartmentNameChanged();
            }
        }
        private global::System.String _DepartmentName;
        partial void OnDepartmentNameChanging(global::System.String value);
        partial void OnDepartmentNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> EntryById
        {
            get
            {
                return _EntryById;
            }
            set
            {
                OnEntryByIdChanging(value);
                ReportPropertyChanging("EntryById");
                _EntryById = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("EntryById");
                OnEntryByIdChanged();
            }
        }
        private Nullable<global::System.Int32> _EntryById;
        partial void OnEntryByIdChanging(Nullable<global::System.Int32> value);
        partial void OnEntryByIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> EntryDate
        {
            get
            {
                return _EntryDate;
            }
            set
            {
                OnEntryDateChanging(value);
                ReportPropertyChanging("EntryDate");
                _EntryDate = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("EntryDate");
                OnEntryDateChanged();
            }
        }
        private Nullable<global::System.DateTime> _EntryDate;
        partial void OnEntryDateChanging(Nullable<global::System.DateTime> value);
        partial void OnEntryDateChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> UpdateBy
        {
            get
            {
                return _UpdateBy;
            }
            set
            {
                OnUpdateByChanging(value);
                ReportPropertyChanging("UpdateBy");
                _UpdateBy = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("UpdateBy");
                OnUpdateByChanged();
            }
        }
        private Nullable<global::System.Int32> _UpdateBy;
        partial void OnUpdateByChanging(Nullable<global::System.Int32> value);
        partial void OnUpdateByChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> UpdatedDate
        {
            get
            {
                return _UpdatedDate;
            }
            set
            {
                OnUpdatedDateChanging(value);
                ReportPropertyChanging("UpdatedDate");
                _UpdatedDate = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("UpdatedDate");
                OnUpdatedDateChanged();
            }
        }
        private Nullable<global::System.DateTime> _UpdatedDate;
        partial void OnUpdatedDateChanging(Nullable<global::System.DateTime> value);
        partial void OnUpdatedDateChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                OnIsActiveChanging(value);
                ReportPropertyChanging("IsActive");
                _IsActive = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("IsActive");
                OnIsActiveChanged();
            }
        }
        private Nullable<global::System.Boolean> _IsActive;
        partial void OnIsActiveChanging(Nullable<global::System.Boolean> value);
        partial void OnIsActiveChanged();

        #endregion

    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="MVCProjectModel", Name="TblEmployee")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class TblEmployee : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new TblEmployee object.
        /// </summary>
        /// <param name="employeeId">Initial value of the EmployeeId property.</param>
        public static TblEmployee CreateTblEmployee(global::System.Int32 employeeId)
        {
            TblEmployee tblEmployee = new TblEmployee();
            tblEmployee.EmployeeId = employeeId;
            return tblEmployee;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 EmployeeId
        {
            get
            {
                return _EmployeeId;
            }
            set
            {
                if (_EmployeeId != value)
                {
                    OnEmployeeIdChanging(value);
                    ReportPropertyChanging("EmployeeId");
                    _EmployeeId = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("EmployeeId");
                    OnEmployeeIdChanged();
                }
            }
        }
        private global::System.Int32 _EmployeeId;
        partial void OnEmployeeIdChanging(global::System.Int32 value);
        partial void OnEmployeeIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                OnFirstNameChanging(value);
                ReportPropertyChanging("FirstName");
                _FirstName = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("FirstName");
                OnFirstNameChanged();
            }
        }
        private global::System.String _FirstName;
        partial void OnFirstNameChanging(global::System.String value);
        partial void OnFirstNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                OnLastNameChanging(value);
                ReportPropertyChanging("LastName");
                _LastName = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("LastName");
                OnLastNameChanged();
            }
        }
        private global::System.String _LastName;
        partial void OnLastNameChanging(global::System.String value);
        partial void OnLastNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Email
        {
            get
            {
                return _Email;
            }
            set
            {
                OnEmailChanging(value);
                ReportPropertyChanging("Email");
                _Email = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Email");
                OnEmailChanged();
            }
        }
        private global::System.String _Email;
        partial void OnEmailChanging(global::System.String value);
        partial void OnEmailChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Password
        {
            get
            {
                return _Password;
            }
            set
            {
                OnPasswordChanging(value);
                ReportPropertyChanging("Password");
                _Password = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Password");
                OnPasswordChanged();
            }
        }
        private global::System.String _Password;
        partial void OnPasswordChanging(global::System.String value);
        partial void OnPasswordChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> JoiningDate
        {
            get
            {
                return _JoiningDate;
            }
            set
            {
                OnJoiningDateChanging(value);
                ReportPropertyChanging("JoiningDate");
                _JoiningDate = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("JoiningDate");
                OnJoiningDateChanged();
            }
        }
        private Nullable<global::System.DateTime> _JoiningDate;
        partial void OnJoiningDateChanging(Nullable<global::System.DateTime> value);
        partial void OnJoiningDateChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String PhoneNumber
        {
            get
            {
                return _PhoneNumber;
            }
            set
            {
                OnPhoneNumberChanging(value);
                ReportPropertyChanging("PhoneNumber");
                _PhoneNumber = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("PhoneNumber");
                OnPhoneNumberChanged();
            }
        }
        private global::System.String _PhoneNumber;
        partial void OnPhoneNumberChanging(global::System.String value);
        partial void OnPhoneNumberChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String AlternatePhoneNumber
        {
            get
            {
                return _AlternatePhoneNumber;
            }
            set
            {
                OnAlternatePhoneNumberChanging(value);
                ReportPropertyChanging("AlternatePhoneNumber");
                _AlternatePhoneNumber = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("AlternatePhoneNumber");
                OnAlternatePhoneNumberChanged();
            }
        }
        private global::System.String _AlternatePhoneNumber;
        partial void OnAlternatePhoneNumberChanging(global::System.String value);
        partial void OnAlternatePhoneNumberChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                OnIsActiveChanging(value);
                ReportPropertyChanging("IsActive");
                _IsActive = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("IsActive");
                OnIsActiveChanged();
            }
        }
        private Nullable<global::System.Boolean> _IsActive;
        partial void OnIsActiveChanging(Nullable<global::System.Boolean> value);
        partial void OnIsActiveChanged();

        #endregion

    
    }

    #endregion

    
}
