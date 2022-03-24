import request from '@/utils/request'

// 查询生成表数据
export function listTable(query) {
  return request({
    url: '/tool/gen/list',
    method: 'get',
    params: query,
  })
}
// 查询db数据库列表
export function listDbTable(query) {
  return request({
    url: '/tool/gen/getTableList',
    method: 'get',
    params: query,
  })
}
/**
 * 获取数据库
 */
export function codeGetDBList() {
  return request({
    url: 'tool/gen/getDbList',
    method: 'get',
  })
}
listDbTable
// 查询表详细信息
export function getGenTable(tableId) {
  return request({
    url: '/tool/gen/' + tableId,
    method: 'get',
  })
}

// 修改代码生成信息
export function updateGenTable(data) {
  return request({
    url: '/tool/gen/',
    method: 'put',
    data: data,
  })
}

// 导入表
export function importTable(data) {
  return request({
    url: '/tool/gen/importTable',
    method: 'post',
    params: data,
  })
}

// 预览生成代码
export function previewTable(tableId) {
  return request({
    url: '/tool/gen/preview/' + tableId,
    method: 'post',
  })
}

// 删除表数据
export function delTable(tableId) {
  return request({
    url: '/tool/gen/' + tableId,
    method: 'delete',
  })
}

// 生成代码（自定义路径）
export function genCode(data) {
  return request({
    url: '/tool/gen/genCode',
    method: 'post',
    data:data,
  })
}

// // 同步数据库
// export function synchDb(tableName) {
//   return request({
//     url: '/tool/gen/synchDb/' + tableName,
//     method: 'get',
//   })
// }

// 同步数据库
export function synchDb(tableId, data) {
  return request({
    url: '/tool/gen/synchDb/' + tableId,
    method: 'get',
		params: data
  })
}