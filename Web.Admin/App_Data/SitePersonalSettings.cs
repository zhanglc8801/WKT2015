using System;
using System.Collections.Generic;

namespace Web.Admin
{
    public class SitePersonalSettings
    {

        /// <summary>
        /// ��ʶ
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// վ��ID
        /// </summary>
        public string SiteID { get; set; }

        /// <summary>
        /// �༭ID
        /// </summary>
        public string EditorID { get; set; }

        /// <summary>
        /// �ڡ��������ר�������ݴ���ʱ��(Ĭ�ϸ��ݸ�����)�Ը������
        /// </summary>
        public byte Personal_Order { get; set; }

        /// <summary>
        /// �ڡ����������ҳ�����ʾ�ҵĸ��
        /// </summary>
        public byte Personal_OnlyMySearch { get; set; }

        

    }
}