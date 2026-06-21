import { Grid, Text, Tooltip } from "@mantine/core";
import { IconCheck, IconX } from "@tabler/icons-react";

import PageSection from "../../components/PageSection";

interface AccountDetailsProps {
  username: string;
  email: string;
  phoneNumber?: string;
  emailConfirmed: boolean;
  phoneNumberConfirmed: boolean;
}

export default function AccountDetails({
  username,
  email,
  phoneNumber,
  emailConfirmed,
  phoneNumberConfirmed,
}: AccountDetailsProps) {
  return (
    <PageSection
      title="Account Details"
      subheader="Here you can find the basic information account your account"
      collapsable
    >
      <Grid w={"100%"} maw={600}>
        <Grid.Col span={5.5}>
          <Text ta={"right"}>Username</Text>
        </Grid.Col>
        <Grid.Col span={5.5}>
          <Text ta={"left"}>{username}</Text>
        </Grid.Col>
        <Grid.Col span={1} />

        <Grid.Col span={5.5}>
          <Text ta={"right"}>Email</Text>
        </Grid.Col>
        <Grid.Col span={5.5}>
          <Text ta={"left"}>{email}</Text>
        </Grid.Col>
        <Grid.Col span={1}>
          {emailConfirmed ? (
            <Tooltip label="Email confirmed">
              <IconCheck size={16} />
            </Tooltip>
          ) : (
            <IconX size={16} />
          )}
        </Grid.Col>

        <Grid.Col span={5.5}>
          <Text ta={"right"}>Phone Number</Text>
        </Grid.Col>
        <Grid.Col span={5.5}>
          <Text ta={"left"}>{phoneNumber}</Text>
        </Grid.Col>
        <Grid.Col span={1}>
          {phoneNumber &&
            (phoneNumberConfirmed ? (
              <Tooltip label="Phone number confirmed">
                <IconCheck size={16} />
              </Tooltip>
            ) : (
              <IconX size={16} />
            ))}
        </Grid.Col>
      </Grid>
    </PageSection>
  );
}
