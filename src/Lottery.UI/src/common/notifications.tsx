import { MantineColor } from "@mantine/core";
import { NotificationData, notifications } from "@mantine/notifications";
import {
  IconCircleCheck,
  IconInfoCircle,
  IconXboxX,
  ReactNode,
} from "@tabler/icons-react";

type NotificationType = "success" | "error" | "info";

function getIcon(type: NotificationType): ReactNode {
  switch (type) {
    case "error":
      return <IconXboxX />;
    case "info":
      return <IconInfoCircle />;
    case "success":
      return <IconCircleCheck />;
    default:
      return null;
  }
}

function getColor(type: NotificationType): MantineColor {
  switch (type) {
    case "error":
      return "red";
    case "info":
      return "blue";
    case "success":
      return "green";
  }
}

interface ShowNotificationParams extends NotificationData {
  type: NotificationType;
}

export function showNotification(params: ShowNotificationParams) {
  return notifications.show({
    ...params,
    autoClose: params.autoClose ?? 5000,
    icon: params.icon ?? getIcon(params.type),
    color: params.color ?? getColor(params.type),
  });
}

export function notifySuccess(params: NotificationData) {
  return showNotification({ type: "success", ...params });
}

export function notifyError(params: NotificationData) {
  return showNotification({ type: "error", ...params });
}

export function notifyInfo(params: NotificationData) {
  return showNotification({ type: "info", ...params });
}

export function updateNotifySuccess(id: string, params: NotificationData) {
  return showNotification({ type: "success", ...params, id });
}

export function updateNotifyError(id: string, params: NotificationData) {
  return showNotification({ type: "error", ...params, id });
}

export function updateNotifyInfo(id: string, params: NotificationData) {
  return showNotification({ type: "info", ...params, id });
}
