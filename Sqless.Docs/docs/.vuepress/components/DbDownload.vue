<template>
  <div>
    <a class="action-button" @click="handleDownload">{{ text }}</a>
  </div>
</template>

<script>
export default {
  props: {
    text: {
      type: String,
      default: 'SqlServer 备份文件'
    }
  },
  methods: {
    handleDownload() {
      window.location.href = '/sqless_db.zip'
    }
  }
}
</script>
