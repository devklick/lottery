import { useMantineTheme } from "@mantine/core";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { useVerifyEmail } from "./hooks";
import { verifyEmailPageQuerySchema } from "./schema";
import { notifyError, notifySuccess } from "../../../common/notifications";
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
      notifySuccess({
        title: "Email address verified",
        message: "Your email address has successfully been verified",
      });
    } else {
      notifyError({
        title: "Error verifying email address",
        message: "We ran into a problem while verifying your email address",
      });
    }
  }, [colors.green, colors.red, query.status]);

  return null;
}
