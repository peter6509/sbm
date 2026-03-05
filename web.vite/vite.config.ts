import { fileURLToPath } from "node:url";
import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
//import { visualizer } from 'rollup-plugin-visualizer'
import vueJsx from "@vitejs/plugin-vue-jsx";
// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    // commonjs({
    //   // 這里可以配置插件的選項，例如指定需要轉换的模塊等
    //   include: 'node_modules/**', // 包含node_modules下的所有模塊
    // }),
    {
      name: "remove-pure-annotations",
      transform(code) {
        return code.replace(/\/\*#__PURE__\*\//g, "");
      },
    },
    vue(),
    vueJsx(),
    // visualizer({
    // //  filename: 'report_form_rollup-plugin-visualizer.html'
    // })
    //引用后提示：
    // requireTransform({
    //   fileRegex: /.js$|.ts$/
    // })
  ],
  define: {
    //這里配置解决打印插件打包異常
   // eval: "window.eval",
    __VUE_PROD_HYDRATION_MISMATCH_DETAILS__: "true",
  },
  optimizeDeps: {
    exclude: ["vue"],
  },
  // build: {
  //   minify: 'terser', // 使用 terser 以獲得更精確的控制
  //   terserOptions: {
  //     exclude: [
  //       /node_modules\/vue-plugin-hiprint/, // 排除特定插件
  //       // 可以添加其他需要排除的模塊
  //     ],
  //     compress: {
  //       defaults: true,
  //       drop_console: true,
  //     },
  //     mangle: true,
  //   },
  // },
  // base: '/test/',
  build: {
    //minify: false,
    //minify: "terser", // 使用 terser 以獲得更精確的控制
    commonjsOptions: {
      transformMixedEsModules: true,
    },
    chunkSizeWarningLimit: 1000, // 提高警告阈值
    rollupOptions: {
      onwarn(warning, warn) {
        if (warning.code === "EMPTY_BUNDLE"||warning.code === 'EVAL'||warning.message.includes('larger than')) {
          return;
        }
        warn(warning);
      },
      // external: ["vue-plugin-hiprint"],
      output: {
         // 保留formatter變量名
        //minifyInternalExports: false,
        //每個node_modules模塊分成一個js文件
        manualChunks(id) {
          if (id.includes("node_modules")) {
            return id
              .toString()
              .split("node_modules/")[1]
              .split("/")[0]
              .toString();
          }
        },
        // 用于從入口點創建的塊的打包輸出格式[name]表示文件名,[hash]表示该文件内容hash值
        // entryFileNames: 'assets/js/[name].[hash].js', // 用于命名代碼拆分時創建的共享塊的輸出命名
        // chunkFileNames: 'assets/js/[name].[hash].js', // 用于輸出静態资源的命名，[ext]表示文件擴展名
        // assetFileNames: 'assets/[ext]/[name].[hash].[ext]'
        entryFileNames: "assets/js/[hash].js", // 用于命名代碼拆分時創建的共享塊的輸出命名
        chunkFileNames: "assets/js/[hash].js", // 用于輸出静態资源的命名，[ext]表示文件擴展名
        assetFileNames: "assets/[ext]/[hash].[ext]",
      },
    },
    // outDir: 'dist', // 輸出目錄，默認為 'dist'
    // rollupOptions: {
    //   input: {
    //     main: 'src/main.js', // 你的入口文件
    //   }
    // },
    sourcemap: false, //生成源映射文件
  },
  publicDir: "public", // 這里指定了 public 文件夹的位置
  css: {
    //查看CSS屬于哪個css文件
    devSourcemap: false,
  },
  server: {
    port: 9000,
    //配置代理
    // proxy: {//http.js里設置為空axios.defaults.baseURL = "";
    //   '/api': {
    //     target:'http://proapi.volcore.xyz/',
    //     changeOrigin: true,
    //     timeout:1000*60*10
    //     //rewrite: (path) => path.replace(/^\/api/, '')
    //   }
    // }
  },
  resolve: {
    extensions: [".mjs", ".js", ".ts", ".jsx", ".tsx", ".json", ".vue", ".mjs"],
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
});
