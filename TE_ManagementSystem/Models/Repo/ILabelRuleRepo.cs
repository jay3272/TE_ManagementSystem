﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE_ManagementSystem.Models.Repo
{
    interface ILabelRuleRepo
    {
        //列出清單
        IQueryable<LabelRule> ListAllLabelRule();

        //列出某個紀錄
        LabelRule GetLabelRuleById(int id);

        //搜尋
        LabelRule GetLabelRuleByName(string name);

        //新增
        bool AddLabelRule(LabelRule labelRule);
        //更新
        bool UpdateLabelRule(LabelRule labelRule);

        //刪除
        bool DeleteLabelRule(LabelRule labelRule);

    }
}
