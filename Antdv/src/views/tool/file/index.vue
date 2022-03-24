<template>
  <page-header-wrapper>
    <a-card :bordered="false">
      <!-- 条件搜索 -->
      <div class="table-page-search-wrapper">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="24">
              <a-form-item label="文件名">
                <a-input v-model="queryParam.fileName" placeholder="请输入" allow-clear/>
              </a-form-item>
            </a-col>
         
            <a-col :md="8" :sm="24">
                <a-form-item label="存储类型">
                  <a-select placeholder="请选择" v-model="queryParam.storeType" style="width: 100%" allow-clear>
                    <a-select-option v-for="(d, index) in storeTypeOptions" :key="index" :value="d.dictValue">{{ d.dictLabel }}</a-select-option>
                  </a-select>
                </a-form-item>
              </a-col>

          
            <a-col :md="!advanced && 8 || 24" :sm="24">
              <span class="table-page-search-submitButtons" :style="advanced && { float: 'right', overflow: 'hidden' } || {} ">
                <a-button type="primary" @click="handleQuery"><a-icon type="search" />查询</a-button>
                <a-button style="margin-left: 8px" @click="resetQuery"><a-icon type="redo" />重置</a-button>
                <a @click="toggleAdvanced" style="margin-left: 8px">
                  {{ advanced ? '收起' : '展开' }}
                  <a-icon :type="advanced ? 'up' : 'down'"/>
                </a>
              </span>
            </a-col>
          </a-row>
        </a-form>
      </div>
      <!-- 操作 -->
      <div class="table-operations">
        <a-button type="primary" @click="$refs.createForm.handleAdd()" v-hasPermi="['system:post:add']">
          <a-icon type="upload" />上传
        </a-button>
       
        <a-button type="danger" :disabled="multiple" @click="handleDelete" v-hasPermi="['system:post:remove']">
          <a-icon type="delete" />删除
        </a-button>
      
        <a-button
          type="dashed"
          shape="circle"
          :loading="loading"
          :style="{float: 'right'}"
          icon="reload"
          @click="getList" />
      </div>
      <!-- 增加修改 -->
      <create-form
        ref="createForm"
        :storeTypeOptions="storeTypeOptions"
        @ok="getList"
      />
      <!-- 数据展示 -->
      <a-table
        :loading="loading"
        :size="tableSize"
        rowKey="id"
        :columns="columns"
        :data-source="list"
        :row-selection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
        :pagination="false">
        <span slot="storeType" slot-scope="text, record">
          {{ storeTypeFormat(record) }}
        </span>
        <span slot="createTime" slot-scope="text, record">
          {{ parseTime(record.createTime) }}
        </span>
        <span slot="operation" slot-scope="text, record">
          
          <a-divider type="vertical" v-hasPermi="['tool:file:delete']" />
          <a @click="handleDelete(record)" v-hasPermi="['tool:file:delete']">
            <a-icon type="delete" />删除
          </a>
        </span>
      </a-table>
      <!-- 分页 -->
      <a-pagination
        class="ant-table-pagination"
        show-size-changer
        show-quick-jumper
        :current="queryParam.pageNum"
        :total="total"
        :page-size="queryParam.pageSize"
        :showTotal="total => `共 ${total} 条`"
        @showSizeChange="onShowSizeChange"
        @change="changeSize"
      />
    </a-card>
  </page-header-wrapper>
</template>

<script>
 import { listSysfile, delSysfile, getSysfile } from "@/api/tool/file.js";
 import CreateForm from './modules/CreateForm'

export default {
  name: 'Post',
  components: {
    CreateForm
  },
  data () {
    return {
      list: [],
      selectedRowKeys: [],
      selectedRows: [],
      // 高级搜索 展开/关闭
      advanced: false,
      // 非单个禁用
      single: true,
      // 非多个禁用
      multiple: true,
      ids: [],
      loading: false,
      total: 0,
      // 状态数据字典
      storeTypeOptions: [],
      queryParam: {
        pageNum: 1,
        pageSize: 10,
        storeType: undefined,
        fileName:undefined
      },
      columns: [
      
        {
          title: 'id',
          dataIndex: 'id',
          align: 'center'
        },
        {
          title: '文件名',
          dataIndex: 'fileName',
          ellipsis: true,
          align: 'center'
        },
        {
          title: '真实名',
          dataIndex: 'realName',
          ellipsis: true,
          align: 'center'
        },
        {
          title: '文件存储地址',
          dataIndex: 'fileUrl',
          ellipsis: true,
          align: 'center'
        },
        {
          title: '文件大小',
          dataIndex: 'fileSize',
          align: 'center'
        },
        {
          title: '文件扩展名',
          dataIndex: 'fileExt',
          align: 'center'
        },
        {
          title: '创建时间',
          dataIndex: 'create_time',
          ellipsis: true,
          scopedSlots: { customRender: 'create_time' },
          align: 'center'
        },
        {
          title: '状态',
          dataIndex: 'storeType',
          scopedSlots: { customRender: 'storeType' },
          align: 'center'
        },
        {
          title: '操作',
          dataIndex: 'operation',
          width: '15%',
          scopedSlots: { customRender: 'operation' },
          align: 'center'
        }
      ]
    }
  },
  filters: {
  },
  created () {
    this.getList()
    this.getDicts('storeType').then(response => {
      this.storeTypeOptions = response.data
    })
  },
  computed: {
  },
  watch: {
  },
  methods: {
    /** 查询部门列表 */
    getList () {
      this.loading = true
      listSysfile(this.queryParam).then(response => {
          this.list = response.data.result
          this.total = response.data.totalNum
          this.loading = false
        }
      )
    },
    // 岗位状态字典翻译
    storeTypeFormat (row) {
      return this.selectDictLabel(this.storeTypeOptions, row.storeType)
    },
    /** 搜索按钮操作 */
    handleQuery () {
      this.queryParam.pageNum = 1
      this.getList()
    },
    /** 重置按钮操作 */
    resetQuery () {
      this.queryParam = {
        pageNum: 1,
        pageSize: 10,
        fileName: undefined,
        storeType: undefined
      }
      this.handleQuery()
    },
    onShowSizeChange (current, pageSize) {
      this.queryParam.pageSize = pageSize
      this.getList()
    },
    changeSize (current, pageSize) {
      this.queryParam.pageNum = current
      this.queryParam.pageSize = pageSize
      this.getList()
    },
    onSelectChange (selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
      this.ids = this.selectedRows.map(item => item.id)
      this.single = selectedRowKeys.length !== 1
      this.multiple = !selectedRowKeys.length
    },
    toggleAdvanced () {
      this.advanced = !this.advanced
    },
    /** 删除按钮操作 */
    handleDelete (row) {
      var that = this
      const fileIds = row.id || this.ids
      this.$confirm({
        title: '确认删除所选中数据?',
        content: '当前选中编号为' + fileIds + '的数据',
        onOk () {
          return delSysfile(fileIds)
            .then(() => {
              that.onSelectChange([], [])
              that.getList()
              that.$message.success(
                '删除成功',
                3
              )
          })
        },
        onCancel () {}
      })
    },
   
  }
}
</script>
