<template>
  <a-drawer width="35%" :label-col="4" :wrapper-col="14" :visible="open" @close="onClose">
    <a-divider orientation="left">
      <b>{{ formTitle }}</b>
    </a-divider>

    <a-tooltip placement="top">
      <template slot="title">
        <span>比如存储到'/uploads' '如果不填写默认按时间存储eg：/2021/12/16(固定段)</span>
      </template>
      <a-icon type="file" />
    </a-tooltip>
    <a-form-model ref="form" :model="form" :rules="rules">
      <a-form-model-item label="存储文件夹前缀" prop="storePath">
        <a-input v-model="form.storePath" placeholder="请输入" />
      </a-form-model-item>

      <a-form-model-item label="自定文件名" prop="fileName">
        <a-input v-model="form.fileName" placeholder="请输入" />
      </a-form-model-item>

      <a-form-model-item label="存储类型" prop="storeType">
        <a-select placeholder="请选择" v-model="form.storeType">
          <a-select-option v-for="(d, index) in storeTypeOptions" :key="index" :value="d.dictValue">{{
            d.dictLabel
          }}</a-select-option>
        </a-select>
      </a-form-model-item>

      <a-form-item label="附件">
        <div :key="ImgKey">
          <file-upload
          v-model="form.accessUrl"
          :limit="1"
          :fileSize="15"
          :data="{ fileDir: form.storePath, fileName: form.fileName }"
          column="accessUrl"
          @input="uploadSuccess"
        />
        </div>
      </a-form-item>

      <div class="bottom-control">
        <a-space>
          <a-button type="primary" @click="submitForm"> 保存 </a-button>
          <a-button type="dashed" @click="cancel"> 取消 </a-button>
        </a-space>
      </div>
    </a-form-model>
  </a-drawer>
</template>

<script>
//import { getPost, addPost, updatePost } from '@/api/system/post'
import { upload } from '@/api/common'
export default {
  name: 'CreateForm',
  props: {
    storeTypeOptions: {
      type: Array,
      required: true,
    },
  },
  components: {},
  data() {
    return {
      loading: false,
      
      formTitle: '',
      // 表单参数
      form: {},
      rules: {
        accessUrl: [
          {
            required: true,
            message: '上传文件不能为空',
            trigger: 'blur',
          },
        ],
        storeType: [
          {
            required: true,
            message: '存储类型不能为空',
            trigger: 'blur',
          },
        ],
      },
      open: false,
    }
  },

  filters: {},
  created() {},
  computed: {},
  watch: {},
  methods: {
    onClose() {
      this.open = false
    },
    // 取消按钮
    cancel() {
      this.open = false
      this.reset()
    },
    // 表单重置
    reset() {
      this.form = {
        fileName: '',
        fileUrl: '',
        storePath: 'uploads',
        fileSize: 0,
        fileExt: '',
        storeType: undefined,
        accessUrl: '',
      }
      //this.$refs.addFrom.resetFields()
      // this.resetForm('form')
    },
    // 上传成功
    uploadSuccess(columnName, filelist) {
      //  this.form[columnName] = filelist;
      //   this.queryParams.fileId = data.fileId;
      //this.open = false;
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset()
      this.open = true
      this.formTitle = '上传文件'
    },

    /** 提交按钮 */
    submitForm: function () {
      this.$refs.form.validate((valid) => {
        if (valid) {
          this.$message.success('修改成功', 3)
          this.open = false
          this.$emit('ok')
        } else {
          return false
        }
      })
    },
  },
}
</script>
