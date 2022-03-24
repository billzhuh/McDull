<template>
  <a-drawer width="35%" :label-col="4" :wrapper-col="14" :visible="open" @close="onClose">
    <a-divider orientation="left">
      <b>{{ formTitle }}</b>
    </a-divider>
    <a-form-model ref="form" :model="form" :rules="rules">
      <a-form-model-item label="任务名称" prop="name">
        <a-input v-model="form.name" placeholder="请输入任务名称" />
      </a-form-model-item>
      <a-form-model-item label="任务分组" prop="jobGroup">
        <a-select placeholder="请选择" v-model="form.jobGroup">
          <a-select-option v-for="(d, index) in jobGroupOptions" :key="index" :value="d.dictValue">{{
            d.dictLabel
          }}</a-select-option>
        </a-select>
      </a-form-model-item>
      <a-form-model-item label="程序集名称" prop="assemblyName">
        <a-input v-model="form.assemblyName" placeholder="请输入程序集名称" />
      </a-form-model-item>
      <a-form-model-item label="任务类名" prop="className">
        <a-input v-model="form.className" placeholder="请输入任务类名" />
      </a-form-model-item>
      <a-form-model-item label="任务参数" prop="jobParams">
        <a-input v-model="form.jobParams" />
      </a-form-model-item>
      <!-- <a-form-model-item prop="invokeTarget">
        <span slot="label">
          调用方法&nbsp;
          <a-popover placement="topLeft">
            <template slot="content">
              <p>Class类调用示例：<code>com.ruoyi.quartz.task.RyTask.ryParams('ry')</code></p>
              <p>参数说明：支持字符串，布尔类型，长整型，浮点型，整型</p>
            </template>
            <span slot="title"> Bean调用示例：<code>ryTask.ryParams('ry')</code></span>
            <a-icon type="question-circle-o" />
          </a-popover>
        </span>
        <a-input v-model="form.invokeTarget" placeholder="请输入调用目标字符串" />
      </a-form-model-item> -->
      <a-form-model-item label="cron表达式" prop="cron">
        <a-input-search
          v-model="form.cron"
          placeholder="请输入cron执行表达式"
          @search="$refs.genCrontab.show(form.cron)"
        >
          <a-button slot="enterButton">
            生成表达式
            <a-icon type="tool" />
          </a-button>
        </a-input-search>
      </a-form-model-item>
      <!-- <a-form-model-item label="是否并发" prop="concurrent">
        <a-radio-group v-model="form.concurrent" button-style="solid">
          <a-radio-button value="0">允许</a-radio-button>
          <a-radio-button value="1">禁止</a-radio-button>
        </a-radio-group>
      </a-form-model-item>
      <a-form-model-item label="错误策略" prop="misfirePolicy">
        <a-radio-group v-model="form.misfirePolicy" button-style="solid">
          <a-radio-button value="1">立即执行</a-radio-button>
          <a-radio-button value="2">执行一次</a-radio-button>
          <a-radio-button value="3">放弃执行</a-radio-button>
        </a-radio-group>
      </a-form-model-item> -->
      <a-form-model-item label="状态" prop="status">
        <a-radio-group v-model="form.status" button-style="solid">
          <a-radio v-for="(d, index) in statusOptions" :key="index" :value="d.dictValue">{{ d.dictLabel }}</a-radio>
        </a-radio-group>
      </a-form-model-item>

      <a-col :span="12">
        <a-form-model-item label="开始日期" prop="beginTime">
          <a-date-picker v-model="form.beginTime" placeholder="请输入开始日期">
            <template slot="renderExtraFooter"> extra footer </template>
          </a-date-picker>
        </a-form-model-item>
      </a-col>

      <a-col :span="12">
        <a-form-model-item label="结束日期" prop="endTime">
          <a-date-picker v-model="form.endTime" placeholder="请输入结束日期">
            <template slot="renderExtraFooter"> extra footer </template>
          </a-date-picker>
        </a-form-model-item>
      </a-col>

      <div class="bottom-control">
        <a-space>
          <a-button type="primary" @click="submitForm"> 保存 </a-button>
          <a-button type="dashed" @click="cancel"> 取消 </a-button>
        </a-space>
      </div>
    </a-form-model>
    <gen-crontab ref="genCrontab" @fill="crontabFill" />
  </a-drawer>
</template>

<script>
import { getJob, addJob, updateJob } from '@/api/monitor/job'
import GenCrontab from './GenCrontab'

export default {
  name: 'CreateForm',
  components: {
    GenCrontab,
  },
  props: {
    statusOptions: {
      type: Array,
      required: true,
    },
    jobGroupOptions: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      loading: false,
      formTitle: '',
      // 表单参数
      form: {
        id: undefined,
        name: undefined,
        jobGroup: undefined,
        assemblyName: undefined,
        className: undefined,
        jobParams: undefined,
        //invokeTarget: undefined,
        cron: undefined,
       endTime: undefined,
        beginTime: undefined,
        isStart: '0',
      },
      open: false,
      openView: false,
      rules: {
        name: [{ required: true, message: '任务名称不能为空', trigger: 'blur' }],
        assemblyName: [{ required: true, message: '任务名称不能为空', trigger: 'blur' }],
        className: [{ required: true, message: '任务名称不能为空', trigger: 'blur' }],
        // invokeTarget: [{ required: true, message: '调用目标字符串不能为空', trigger: 'blur' }],
        cron: [{ required: true, message: 'cron执行表达式不能为空', trigger: 'blur' }],
      },
    }
  },
  filters: {},
  created() {},
  computed: {},
  watch: {},
  methods: {
    handleView(row) {
      getJob(row.jobId).then((response) => {
        this.form = response.data
        this.openView = true
      })
    },
    onCloseView() {
      this.openView = false
    },
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
        id: undefined,
        name: undefined,
        jobGroup: undefined,
       jobParams: undefined,
        cron: undefined,
        endTime: undefined,
        beginTime: undefined,
        isStart: '0',
      }
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset()
      this.open = true
      this.formTitle = '添加任务'
    },
    /** 修改按钮操作 */
    handleUpdate(row, ids) {
      this.reset()
      //  console.log('------'+row.id)
      // console.log('------'+ids)
      const jobId = row ? row.id : ids
      getJob(jobId).then((response) => {
        this.form = response.data
        this.open = true
        this.formTitle = '修改任务'
      })
    },
    /** 提交按钮 */
    submitForm: function () {
      this.$refs.form.validate((valid) => {
        if (valid) {
          if (this.form.jobId !== undefined) {
            updateJob(this.form).then((response) => {
              this.$message.success('修改成功', 3)
              this.open = false
              this.$emit('ok')
            })
          } else {
            addJob(this.form).then((response) => {
              this.$message.success('新增成功', 3)
              this.open = false
              this.$emit('ok')
            })
          }
        } else {
          return false
        }
      })
    },
    crontabFill(value) {
      this.form.cronExpression = value
    },
  },
}
</script>
