<template>
  <page-header-wrapper >
    
    <a-card :bordered="false">
      <a-row :gutter="12">
        <a-col :span="12">
          <a-form-model ref="form" :model="form" :rules="rules" >

              <a-form-model-item v-for="(domain, index) in form.toEmails" :prop="'toEmails.' + index + '.value'" :label="'收件邮箱' + (index === 0 ? '': index)"
                :key="domain.key"
                 :rules="[{ required: true, message: '邮箱不能为空', trigger: 'blur' }, { message: '请输入正确的邮箱地址', trigger: ['blur', 'change'], type: 'email', }]">
                <a-input v-model="domain.value" style="width:300px"></a-input>
                <a-button class="ml10" @click="addDomain" icon="plus" />
                <a-button class="ml10" @click.prevent="removeDomain(domain)" icon="minus" />
             </a-form-model-item>

            <a-form-model-item  label='邮件主题' prop="subject" >
              <a-input s v-model="form.subject" placeholder="请输入主题" />
            </a-form-model-item>

            <a-form-model-item prop="htmlContent" label='邮件内容'>
              <editor ref="noticeContentEditor" v-model="form.htmlContent" />
            </a-form-model-item>

            <a-form-item label="发送自己" prop="sendMe">
                <a-switch v-model="form.sendMe" active-text="是" inactive-text="否"></a-switch>
            </a-form-item>

            <a-form-item label="附件">
               <file-upload v-model="form.fileUrl" :limit="5" :fileSize="15" :data="{ 'fileDir' : 'email', 'uploadType': 1}" column="fileUrl"
          @input="uploadSuccess" />
            </a-form-item>

            <a-form-item>
                    <a-button type="primary"  @click="formSubmit">发送邮件</a-button>
            </a-form-item>

          </a-form-model>
        </a-col>
      </a-row>
    </a-card>
  
  </page-header-wrapper>
</template>

<script>

import { sendEmail } from "@/api/common";
import Editor from '@/components/Editor'

export default {
  name: "sendEmail",
    components: {
    Editor
  },
  data() {
    return {
      form: {
        fileUrl: "",
        toEmails: [
          {
            value: "",
          },
        ],
      },
      uploadActionUrl: process.env.VUE_APP_BASE_API + "common/uploadFile",
      rules: {
        subject: [{ required: true, message: "主题不能为空", trigger: "blur" }],
        content: [{ required: true, message: "内容不能为空", trigger: "blur" }],
      },
    };
  },
  methods: {
    // 表单重置
    reset() {
      this.form = {
        toUser: undefined,
        htmlContent: undefined,
        subject: undefined,
        fileUrl: undefined,
        sendMe: false,
        toEmails: [
          {
            value: "",
          },
        ],
      };
      this.resetForm("form");
    },
    // 上传成功
    uploadSuccess(columnName, filelist) {
      this.form[columnName] = filelist;
    },
    /**
     * 提交
     */
    formSubmit() {
      this.$refs["form"].validate((valid) => {
        //开启校验
        if (valid) {
        //   const loading = this.$loading({
        //     lock: true,
        //     text: "Loading",
        //     spinner: "loading",
        //     background: "rgba(0, 0, 0, 0.7)",
        //   });
           
          var emails = [];
          this.form.toEmails.filter((x) => {
            emails.push(x.value);
          });
           
          var p = {
            ...this.form,
            toUser: emails.toString(),
          };
          // 如果校验通过，请求接口，允许提交表单
          sendEmail(p).then((res) => {
             
            this.open = false;
            if (res.code == 200) {
              this.$message.success("发送成功");
              this.reset();
            }
            loading.close();
          });
        //   setTimeout(() => {
        //     loading.close();
        //   }, 10000);
        } else {
          console.log("未通过");
          //校验不通过
          return false;
        }
      });
    },
    removeDomain(item) {
      var index = this.form.toEmails.indexOf(item);
      if (index !== -1 && this.form.toEmails.length !== 1) {
        this.form.toEmails.splice(index, 1);
      } else {
        this.$message({
          message: "请至少保留一位联系人",
          type: "warning",
        });
      }
    },
    addDomain() {
      this.form.toEmails.push({
        value: "",
        key: Date.now(),
      });
    },
  },
};
</script>
<style scoped>
.el-upload-list {
  width: 200px;
}
</style>
