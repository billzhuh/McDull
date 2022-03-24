<template>
  <a-modal
    ref="createModal"
    :title="'导入代码'"
    :width="900"
    :visible="visible"
    @cancel="close"
    @ok="confirm"
    :confirmLoading="confirmLoading"
  >
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row :gutter="48">
          <a-col :md="8" :sm="24">
            <a-form-item label="数据库" prop="dbName">
              <a-select v-model="queryParam.dbName" clearable placeholder="请选择" @change="handleShowTable">
                <a-select-option v-for="item in dbList" :key="item" :label="item" :value="item">
                  {{ item }}
                </a-select-option>
              </a-select>
            </a-form-item>
          </a-col>

          <a-col :md="8" :sm="24">
            <a-form-item label="表名称">
              <a-input v-model="queryParam.tableName" placeholder="请输入" allow-clear />
            </a-form-item>
          </a-col>

          <a-col :md="8" :sm="24">
            <span class="table-page-search-submitButtons">
              <a-button @click="handleQuery" type="primary">查询</a-button>
              <a-button @click="resetQuery" style="margin-left: 8px">重置</a-button>
            </span>
          </a-col>
        </a-row>
      </a-form>
    </div>
    <div class="page-header-content">
      <a-card :bordered="false" class="content">
        <a-table
          :loading="loading"
          :size="tableSize"
          rowKey="tableName"
          :columns="columns"
          :data-source="list"
          :row-selection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
          :scroll="{ y: tableHeight }"
          :pagination="false"
        >
          <span slot="createTime" slot-scope="text, record">
            {{ parseTime(record.createTime) }}
          </span>
          <span slot="updateTime" slot-scope="text, record">
            {{ parseTime(record.updateTime) }}
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
          :showTotal="(total) => `共 ${total} 条`"
          @showSizeChange="onShowSizeChange"
          @change="changeSize"
        />
      </a-card>
    </div>
  </a-modal>
</template>
<script>
import { listDbTable, importTable, codeGetDBList } from '@/api/tool/gen'
export default {
  name: 'ImportTable',
  props: {},
  data() {
    return {
      // 表格数据
      list: [],
      selectedRowKeys: [],
      selectedRows: [],
      // 表格的高度
      tableHeight: document.documentElement.scrollHeight - 500 + 'px',
      dbList: [],
      // 选中表数组
      tableNames: [],
      loading: false,
      total: 0,
      // 当前控件配置:
      confirmLoading: false,
      visible: false,
      // 查询参数
      queryParam: {
        pageNum: 1,
        pageSize: 10,
        dbName: '',
        tableName: undefined,
        tableComment: undefined,
      },
      // 表格属性
      columns: [
        {
          title: '表名称',
          dataIndex: 'name',
          ellipsis: true,
          align: 'center',
        },
        {
          title: '表描述',
          dataIndex: 'description',
          ellipsis: true,
          align: 'center',
        },
        // {
        //   title: '创建时间',
        //   dataIndex: 'createTime',
        //   scopedSlots: { customRender: 'createTime' },
        //   align: 'center',
        // },
        // {
        //   title: '更新时间',
        //   dataIndex: 'updateTime',
        //   scopedSlots: { customRender: 'updateTime' },
        //   align: 'center',
        // },
      ],
    }
  },

  created() {
    this.getList()
  },
  methods: {
    handleChange(selectedItems) {
      this.selectedItems = selectedItems
    },

    // 查询表数据
    // 查询表数据
    getList() {
      codeGetDBList().then((res) => {
        this.dbList = res.data.dbList
      
      })
      if (this.queryParam.dbName != '') {
        listDbTable(this.queryParam).then((res) => {
          //this.dbTableList = res.data.dbList;
          this.list = res.data.result
          this.total = res.data.totalNum
        })
      }
    },
    // 关闭模态框
    close() {
      this.visible = false
      this.selectedRowKeys = []
      this.selectedRows = []
      this.queryParam = {
        pageNum: 1,
        pageSize: 10,
        dbName: undefined,
        tableName: undefined,
        tableComment: undefined,
      }
    },
    // 打开抽屉(由外面的组件调用)
    show() {
      this.visible = true
      this.getList()
    },
    // 确认导入
    confirm() {
      importTable({
        tables: this.tableNames.join(','),
        dbName: this.queryParam.dbName,
      }).then((res) => {
        this.$message.success(res.msg)
        this.visible = false
        this.$emit('ok')
      })
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParam.pageNum = 1
      this.getList()
    },

    handleShowTable() {
      this.handleQuery()
    },
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.tables = selection.map((item) => item.name)
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.queryParam = {
        pageNum: 1,
        pageSize: 10,
        tableName: undefined,
        tableComment: undefined,
      }
      this.handleQuery()
    },
    onShowSizeChange(current, pageSize) {
      this.queryParam.pageSize = pageSize
      this.getList()
    },
    changeSize(current, pageSize) {
      this.queryParam.pageNum = current
      this.queryParam.pageSize = pageSize
      this.getList()
    },
    onSelectChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
      this.tableNames = this.selectedRows.map((item) => item.name)
      this.single = selectedRowKeys.length !== 1
      this.multiple = !selectedRowKeys.length
    },
  },
}
</script>
