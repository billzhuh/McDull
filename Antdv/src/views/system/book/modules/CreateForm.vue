<template>
  <a-drawer width="35%" :label-col="4" :wrapper-col="14" :visible="open" @close="onClose">
    <a-divider orientation="left">
      <b>{{ formTitle }}</b>
    </a-divider>
    <a-form-model ref="form" :model="form" :rules="rules">
      <a-form-model-item label="书名" prop="bookname" >
        <a-input v-model="form.bookname" placeholder="请输入${comment}" />
      </a-form-model-item>
      <a-form-model-item label="单价" prop="bookprice" >
        <a-input v-model="form.bookprice" placeholder="请输入${comment}" />
      </a-form-model-item>
      <a-form-model-item label="作者" prop="bookauthor" >
        <a-input v-model="form.bookauthor" placeholder="请输入${comment}" />
      </a-form-model-item>
      <a-form-model-item label="备注" prop="mark" >
        <a-input v-model="form.mark" placeholder="请输入内容" type="textarea" allow-clear />
      </a-form-model-item>
      <div class="bottom-control">
        <a-space>
          <a-button type="primary" @click="submitForm">
            保存
          </a-button>
          <a-button type="dashed" @click="cancel">
            取消
          </a-button>
        </a-space>
      </div>
    </a-form-model>
  </a-drawer>
</template>

<script>
import { getBook, addBook, updateBook } from '@/api/system/book'

export default {
  name: 'CreateForm',
  props: {
  },
  components: {
  },
  data () {
    return {
      loading: false,
      formTitle: '',
      // 表单参数
      form: {
        bookId: null,

        bookname: null,

        bookprice: null,

        bookauthor: null,

        mark: null

      },
      // 1增加,2修改
      formType: 1,
      open: false,
      rules: {
        bookprice: [
          { required: true, message: '$comment不能为空', trigger: 'blur' }
        ]

      }
    }
  },
  filters: {
  },
  created () {
  },
  computed: {
  },
  watch: {
  },
  mounted () {
  },
  methods: {
    onClose () {
      this.open = false
    },
    // 取消按钮
    cancel () {
      this.open = false
      this.reset()
    },
    // 表单重置
    reset () {
      this.formType = 1
      this.form = {
        bookId: null,

        bookname: null,

        bookprice: null,

        bookauthor: null,

        mark: null

      }
    },
    /** 新增按钮操作 */
    handleAdd (row) {
      this.reset()
      this.formType = 1
      this.open = true
      this.formTitle = '添加'
    },
    /** 修改按钮操作 */
    handleUpdate (row, ids) {
      this.reset()
      this.formType = 2
      const bookId = row ? row.bookId : ids
      getBook(bookId).then(response => {
        this.form = response.data
        this.open = true
        this.formTitle = '修改'
      })
    },
    /** 提交按钮 */
    submitForm: function () {
      this.$refs.form.validate(valid => {
        if (valid) {
          if (this.form.bookId !== undefined && this.form.bookId !== null) {
            updateBook(this.form).then(response => {
              this.$message.success(
                '修改成功',
                3
              )
              this.open = false
              this.$emit('ok')
            })
          } else {
            addBook(this.form).then(response => {
              this.$message.success(
                '新增成功',
                3
              )
              this.open = false
              this.$emit('ok')
            })
          }
        } else {
          return false
        }
      })
    }
  }
}
</script>
