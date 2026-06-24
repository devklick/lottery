import { useMantineTheme } from "@mantine/core";
import { notifications } from "@mantine/notifications";
import { IconCircleCheck, IconXboxX } from "@tabler/icons-react";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { useVerifyEmail } from "./hooks";
import { verifyEmailPageQuerySchema } from "./schema";
import useValidatedQueryParams from "../../../hooks/url/useValidatedSearchParams";

export default function VerifyEmail() {
  const paramValidation = useValidatedQueryParams(verifyEmailPageQuerySchema);
  const navigate = useNavigate();
  const { colors } = useMantineTheme();

  useEffect(() => {
    if (!paramValidation.success) {
      navigate("/home");
    }
  }, [navigate, paramValidation.success]);

  const query = useVerifyEmail(paramValidation.data);

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
