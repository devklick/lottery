import { useMantineTheme } from "@mantine/core";
import { notifications } from "@mantine/notifications";
import { IconCircleCheck, IconXboxX } from "@tabler/icons-react";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { useConfirmEmail } from "./hooks";
import { confirmEmailPageQuerySchema } from "./schema";
import useValidatedQueryParams from "../../../hooks/url/useValidatedSearchParams";

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface ConfirmEmailProps {}

// eslint-disable-next-line no-empty-pattern
export default function ConfirmEmail({}: ConfirmEmailProps) {
  const paramValidation = useValidatedQueryParams(confirmEmailPageQuerySchema);
  const navigate = useNavigate();
  const { colors } = useMantineTheme();

  useEffect(() => {
    if (!paramValidation.success) {
      navigate("/home");
    }
  }, [navigate, paramValidation.success]);

  const query = useConfirmEmail(paramValidation.data);

  useEffect(() => {
    if (query.status === "pending") return;
    else if (query.status === "success") {
      notifications.show({
        title: "Email address verified",
        message: "Your email address has successfully been verified",
        icon: <IconCircleCheck />,
        color: colors.green[5],
      });
    } else {
      notifications.show({
        title: "Error verifying email address",
        message: "We ran into a problem while verifying your email address",
        icon: <IconXboxX />,
        color: colors.red[5],
      });
    }
  }, [colors.green, colors.red, query.status]);

  return null;
}
