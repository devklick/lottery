import PageSection from "../components/PageSection";

interface YourWinsProps {}

export default function YourWins({}: YourWinsProps) {
  return (
    <PageSection
      title="Your Wins"
      subheader="Here you can find all winning entries you have placed"
      collapsable
    />
  );
}
