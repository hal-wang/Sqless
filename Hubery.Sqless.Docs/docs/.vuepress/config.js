module.exports = {
    title: 'Sqless',
    description: 'Sqless',
    head: [
        ['link', { rel: 'icon', href: '/logo.png' }],
    ],
    markdown: {
        lineNumbers: true
    },
    base: '/',
    serviceWorker: true,
    themeConfig: {
        logo: '/logo.png',
        lastUpdated: true,
        nav: [
            { text: '首页', link: '/' },
            {
                text: '使用文档',
                ariaLabel: '使用文档',
                items: [
                    { text: '基本用法', link: '/usage/base/start' },
                    { text: 'Client/Server', link: '/usage/cs/about' },
                ]
            },
            {
                text: '示例',
                ariaLabel: '示例',
                items: [
                    { text: '一个简单的示例项目', link: '/demo/store/' },
                ]
            },
            { text: 'GitHub', link: 'https://github.com/hbrwang/sqless' },
        ],
        sidebar: {
            '/usage/': [
                {
                    title: '单元测试',
                    collapsable: false,
                    sidebarDepth: 2,
                    children: [
                        'unit/'
                    ]
                },
                {
                    title: '基本用法',
                    collapsable: false,
                    sidebarDepth: 2,
                    children: [
                        'base/start',
                        'base/select',
                        'base/count',
                        'base/delete',
                        'base/insert',
                        'base/update',
                        'base/upsert',
                    ]
                },
                {
                    title: 'Client/Server',
                    collapsable: false,
                    sidebarDepth: 2,
                    children: [
                        'cs/about',
                        'cs/webapi',
                        'cs/client',
                        'cs/auth',
                        'cs/access',
                    ]
                },
            ],
            '/demo/': [
                {
                    title: '示例',
                    collapsable: false,
                    sidebarDepth: 2,
                    children: [
                        'store/'
                    ]
                }
            ]
        }
    }
}