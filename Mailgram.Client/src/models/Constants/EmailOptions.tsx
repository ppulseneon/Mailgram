import ConnectCredentials from "../ConnectCredentials.tsx";

const emailOptions: { name: string; smtpCredentials: ConnectCredentials; imapCredentials: ConnectCredentials }[] = [
    {
        name: "Yandex",
        smtpCredentials: {
            hostname: "smtp.yandex.ru",
            port: 465,
        },
        imapCredentials: {
            hostname: "imap.yandex.ru",
            port: 993,
        },
    },
    {
        name: "Rambler",
        smtpCredentials: {
            hostname: "smtp.rambler.ru",
            port: 465,
        },
        imapCredentials: {
            hostname: "imap.rambler.ru",
            port: 993,
        },
    },
];


export default emailOptions;