using System;
using System.Collections.Generic;
using System.Text;
using WKT.Common.Extension;
using com.yeepay.bank;

namespace WKT.Common.Pay
{
    public class YeepayHelper
    {
        /// <summary>
        /// �̻��ź��̻���Կ
        /// </summary>
        private string p1_MerId = "";
        private string keyValue = "";

        public YeepayHelper()
        { }

        public YeepayHelper(string EBankCode, string EBankEncryKey)
        {
            p1_MerId = EBankCode;
            keyValue = EBankEncryKey;
        }

        /// <summary>
        /// ��������֧����ַ
        /// </summary>
        /// <param name="p3_Amt">���</param>
        /// <param name="p5_Pid">��Ʒ����</param>
        /// <param name="p6_Pcat">��Ʒ����</param>
        /// <param name="p7_Pdesc">��Ʒ����</param>
        /// <param name="p8_Url">�ͻ��˽���֧���ɹ������ݵ���ҳ��ַ������·����</param>
        /// <param name="pa_MP">�̻���չ��Ϣ</param>
        /// <returns></returns>
        public string CreateBuyUrl(string p3_Amt, string p5_Pid, string p6_Pcat, string p7_Pdesc, string p8_Url, string pa_MP)
        {
            return Buy.CreateBuyUrl(p1_MerId, keyValue, "", p3_Amt, "CNY", p5_Pid, p6_Pcat, p7_Pdesc, p8_Url, "0", pa_MP, "", "0");
        }

        /// <summary>
        /// ��ȡ֧���ɹ�������Ϣ
        /// </summary>
        /// <param name="p1_MerId">�̻����</param>
        /// <param name="keyValue">�̻���Կ</param>
        /// <returns></returns>
        private BuyCallbackResult GetPayResult()
        {
            BuyCallbackResult payResult = Buy.VerifyCallback(FormatQueryString.GetQueryString("p1_MerId"), keyValue, FormatQueryString.GetQueryString("r0_Cmd"), FormatQueryString.GetQueryString("r1_Code"), FormatQueryString.GetQueryString("r2_TrxId"),
            FormatQueryString.GetQueryString("r3_Amt"), FormatQueryString.GetQueryString("r4_Cur"), FormatQueryString.GetQueryString("r5_Pid"), FormatQueryString.GetQueryString("r6_Order"), FormatQueryString.GetQueryString("r7_Uid"),
            FormatQueryString.GetQueryString("r8_MP"), FormatQueryString.GetQueryString("r9_BType"), FormatQueryString.GetQueryString("rp_PayDate"), FormatQueryString.GetQueryString("hmac"));
            return payResult;
        }

        /// <summary>
        /// ��ȡ֧���ɹ�������Ϣ
        /// </summary>
        /// <param name="actionData"></param>
        /// <param name="actionMsg"></param>
        /// <returns></returns>
        public WKT.Model.FinancePayDetailEntity GetPayResult(Action<String> actionMsg)
        {
            BuyCallbackResult result = GetPayResult();
            if (!string.IsNullOrEmpty(result.ErrMsg))
            {
                actionMsg("����ǩ����Ч��");
                return null;
            }
            if (result.R1_Code.Equals("1"))
            {                
                actionMsg("֧���ɹ���");
                WKT.Model.FinancePayDetailEntity model = new Model.FinancePayDetailEntity();
                //model.BankID = result.R6_Order;
                model.TransactionID = result.R2_TrxId;
                model.Currency = result.R4_Cur;
                model.TotalFee = Convert.ToDecimal(result.R3_Amt);
                model.IsInCome = 1;
                model.PayStatus = 1;
                model.UserAccount = "";
                model.BankID = FormatQueryString.GetQueryString("rb_BankId");
                model.BankNo = FormatQueryString.GetQueryString("ro_BankOrderId");

                string[] arrMP = result.R8_MP.Split(',');
                model.PayType = arrMP[0].TryParse<Byte>();
                model.ProductTable = arrMP[1];
                model.ProductID = arrMP[2];
                model.ProductDes = arrMP[3];
                model.NoticeID = arrMP[4].TryParse<Int64>();
                return model;
            }
            else
            {
                actionMsg("֧��ʧ�ܣ�");
                return null;
            }
        }

        /*
        /// <summary>
        /// ��ѯ��ͨ�����׶����Ų�ѯ������Ϣ
        /// </summary>
        /// <param name="p1_MerId">�̻����</param>
        /// <param name="keyValue">�̻���Կ</param>
        /// <param name="p2_Order">�̻�������</param>
        /// <returns></returns>
        public static BuyQueryOrdDetailResult QueryOrder(string p2_Order)
        {
            BuyQueryOrdDetailResult queryResult = Buy.QueryOrdDetail(p1_MerId, keyValue, p2_Order);

            return queryResult;
        }

        /// <summary>
        /// �˿ͨ���ױ�������ˮ�Ž����˿����
        /// </summary>
        /// <param name="p1_MerId">�̻����</param>
        /// <param name="keyValue">�̻���Կ</param>
        /// <param name="pb_TrxId">�ױ�������ˮ��</param>
        /// <param name="p3_Amt">�˿���</param>
        /// <param name="p5_Desc">�˿�˵��</param>
        /// <returns></returns>
        public static BuyRefundOrdResult RollBackPay(string pb_TrxId, string p3_Amt, string p5_Desc)
        {
            BuyRefundOrdResult rollPay = Buy.RefundOrd(p1_MerId, keyValue, pb_TrxId, p3_Amt, "CNY", p5_Desc);

            return rollPay;
        }
        */
    }
}
