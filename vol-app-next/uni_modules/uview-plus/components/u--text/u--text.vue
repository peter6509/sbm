<template>
    <uvText
        :type="type"
        :show="show"
        :text="text"
        :prefixIcon="prefixIcon"
        :suffixIcon="suffixIcon"
        :mode="mode"
        :href="href"
        :format="format"
        :call="call"
        :openType="openType"
        :bold="bold"
        :block="block"
        :lines="lines"
        :color="color"
		:decoration="decoration"
        :size="size"
        :iconStyle="iconStyle"
        :margin="margin"
        :lineHeight="lineHeight"
        :align="align"
        :wordWrap="wordWrap"
        :customStyle="customStyle"
    ></uvText>
</template>

<script>
/**
 * 此组件存在的理由是，在nvue下，u-text被uni-app官方占用了，u-text在nvue中相當于input组件
 * 所以在nvue下，取名為u--input，内部其實還是u-text.vue，只不過做一層中轉
 * 不使用v-bind="$attrs"，而是分開独立写傳参，是因為微信小程序不支持此写法
 */
import uvText from "../u-text/u-text.vue";
import { props } from "../u-text/props.js";
import { mpMixin } from '../../libs/mixin/mpMixin.js'
import { mixin } from '../../libs/mixin/mixin.js'
export default {
    name: "u--text",
    mixins: [mpMixin, mixin, props,],
    components: {
        uvText,
    },
};
</script>
