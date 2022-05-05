﻿namespace iXlinkerDtos
{
    using System.Collections.ObjectModel;

    public class TopologyStructViewModel : PouStructBase
    {
        private string extends;
        public string Extends
        {
            get { return this.extends; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.extends = value;
                    NotifyPropertyChanged(nameof(Extends));
                }
            }
        }

        private ObservableCollection<TopologyStructMemberViewModel> structMembers;
        public ObservableCollection<TopologyStructMemberViewModel> StructMembers
        {
            get
            {
                return this.structMembers ?? (this.structMembers = new ObservableCollection<TopologyStructMemberViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.structMembers = value;
                    NotifyPropertyChanged(nameof(StructMembers));
                }
            }
        }
        public void AddMemberAndUpdateIdAndSize(TopologyStructMemberViewModel member)
        {
            StructMembers.Add(member);
            Id = Id + member.Name + member.InOutPlcProj + member.Type_Value + member.Size;
            Size = Size + member.Size;
            //Id = Id + member.Name + member.InOutPlcProj + member.Type_Value.Substring(member.Type_Value.LastIndexOf(".") + 1) + member.Size;
        }
    }
}
