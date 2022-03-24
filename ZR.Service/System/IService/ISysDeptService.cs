﻿using System;
using System.Collections.Generic;
using System.Text;
using ZR.Model.System;
using ZR.Model.Vo.System;

namespace ZR.Service.System.IService
{
    public interface ISysDeptService : IBaseService<SysDept>
    {
        List<SysDept> GetSysDepts(SysDept dept);
        string CheckDeptNameUnique(SysDept dept);
        int InsertDept(SysDept dept);
        int UpdateDept(SysDept dept);
        void UpdateDeptChildren(long deptId, string newAncestors, string oldAncestors);
        List<SysDept> GetChildrenDepts(List<SysDept> depts, long deptId);
        List<SysDept> BuildDeptTree(List<SysDept> depts);
        List<TreeSelectVo> BuildDeptTreeSelect(List<SysDept> depts);
    }
}
