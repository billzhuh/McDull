<template>
  <global-footer class="footer custom-render">
    <template v-slot:links>
      <a href="RuoYi-Antdv" target="_blank">Gitee</a>
      <a href="RuoYi-Antdv" target="_blank">Github</a>
    </template>
    <template v-slot:copyright>
      <a href="RuoYi-Antdv" target="_blank">RuoYi-Antdv</a>
    </template>
  </global-footer>
</template>

<script>
import { GlobalFooter } from '@/components/ProLayout'

export default {
  name: 'ProGlobalFooter',
  components: {
    GlobalFooter
  }
}
</script>
