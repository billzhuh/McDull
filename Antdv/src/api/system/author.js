import request from '@/utils/request'

// 查询【请填写功能名称】列表
export function listAuthor (query) {
  return request({
    url: '/system/author/list',
    method: 'get',
    params: query
  })
}

// 查询【请填写功能名称】详细
export function getAuthor (id) {
  return request({
    url: '/system/author/' + id,
    method: 'get'
  })
}

// 新增【请填写功能名称】
export function addAuthor (data) {
  return request({
    url: '/system/author',
    method: 'post',
    data: data
  })
}

// 修改【请填写功能名称】
export function updateAuthor (data) {
  return request({
    url: '/system/author',
    method: 'put',
    data: data
  })
}

// 删除【请填写功能名称】
export function delAuthor (id) {
  return request({
    url: '/system/author/' + id,
    method: 'delete'
  })
}
